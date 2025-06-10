using System.Management.Automation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brimborium.Powershell;

[Cmdlet(VerbsLifecycle.Start, "BrimboriumPowershell")]
public class StartBrimboriumPowershell : PSCmdlet {
    [Parameter]
    public string? ConfigurationPath { get; set; } = null;

    [Parameter]
    public string? Configuration2Path { get; set; } = null;

    [Parameter]
    public SwitchParameter EnableConsoleLogging { get; set; }

    [Parameter]
    public SwitchParameter EnableFileLogging { get; set; }

    [Parameter]
    public SwitchParameter PassThru { get; set; }

    protected override void BeginProcessing() {
        var root = this.GetVariableValue("PSScriptRoot") as string;
        if (string.IsNullOrEmpty(root)) {
            root = this.SessionState.Path.CurrentFileSystemLocation.Path;
        }
        
        ConfigurationBuilder configurationBuilder = new();
        {
            if (this.ConfigurationPath is { Length: > 0 } configurationPath) {
                if (System.IO.Path.Combine(root, configurationPath) is { Length: > 0 } configurationPathRooted) {
                    configurationBuilder.AddJsonFile(configurationPathRooted, optional: false, reloadOnChange: true);
                }
            }
        }
        {
            if (this.Configuration2Path is { Length: > 0 } configurationPath) {
                if (System.IO.Path.Combine(root, configurationPath) is { Length: > 0 } configurationPathRooted) {
                    configurationBuilder.AddJsonFile(configurationPathRooted, optional: false, reloadOnChange: true);
                }
            }
        }
        var latestAssemblyResolver = new LatestAssemblyResolver();
        var plugInDependencyContext = latestAssemblyResolver.PlugInDependencyContext;
        var dependencyContext = Microsoft.Extensions.DependencyModel.DependencyContext.Default;
        if (System.Reflection.Assembly.GetEntryAssembly() is { }  assembly) {
            plugInDependencyContext.Add(assembly, dependencyContext);
        }

        var configuration = configurationBuilder.Build();
        ServiceCollection serviceBuilder = new();
        serviceBuilder.AddSingleton<LatestAssemblyResolver>(latestAssemblyResolver);
        serviceBuilder.AddSingleton<IConfiguration>(configuration);
        serviceBuilder.AddLogging((loggingBuilder) => {
            loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
            if (this.EnableConsoleLogging.ToBool()) {
                loggingBuilder.AddConsole();
            }
            if (this.EnableFileLogging.ToBool()) {
                loggingBuilder.AddLocalFile(configuration.GetSection("Logging:LocalFile"));
            }
        });
        var serviceProvider = serviceBuilder.BuildServiceProvider();
        var bpss = new BrimboriumPowershellSessionState(
            serviceProvider);
        this.SessionState.PSVariable.Set("BrimboriumPowershellSessionState", bpss);

        using (var ps = PowerShell.Create(RunspaceMode.CurrentRunspace)) {
            ps.AddScript(
                """
                Register-EngineEvent -SourceIdentifier PowerShell.Exiting -SupportEvent -Action {
                    Clear-BrimboriumPowershell
                }
                """);
            ps.Invoke();
        }
        if (this.PassThru.ToBool()) {
            this.WriteObject(bpss);
        }
    }
}

/*
foreach (var absolutePluginFilename in listAbsolutePluginFilename) {
            var reader = new DependencyContextJsonReader();
            var absolutePluginDepsFilename = $"{absolutePluginFilename[0..^4]}.deps.json";
            if (!System.IO.File.Exists(absolutePluginDepsFilename)) {
                throw new Exception($"{absolutePluginDepsFilename} not found.");
            }
            using var stream = System.IO.File.OpenRead(absolutePluginDepsFilename);
            var dependencyContextLib = reader.Read(stream);
            if (!(dependencyContextLib is { })) { throw new Exception("dependencyContextLib is null"); }
            var absolutePluginFolder = System.IO.Path.GetDirectoryName(absolutePluginDepsFilename)!;
            plugInDependencyContext.Add(absolutePluginFolder, dependencyContextLib);

            //System.Console.Out.WriteLine(absolutePluginDepsFilename);
            //foreach (var compileLibrary in dependencyContextLib.CompileLibraries) {
            //    System.Console.Out.WriteLine($"  c  {compileLibrary.Name} {compileLibrary.Version}");
            //    AddCompileLibraryByName(dictCompileLibraryByName, compileLibrary);
            //}
            //System.Console.Out.WriteLine(absolutePluginDepsFilename);
            //foreach (var runtimeLibrary in dependencyContextLib.RuntimeLibraries) {
            //    System.Console.Out.WriteLine($"  r  {runtimeLibrary.Name} {runtimeLibrary.Version}");
            //    //AddCompileLibraryByName(dictCompileLibraryByName, runtimeLibrary);
            //}
            //mergedDependencyContext = mergedDependencyContext.Merge(dependencyContextLib);
        }

        {
            System.Console.Out.WriteLine("PlugInDependencyContext");
            foreach (var runtimeFileByName in plugInDependencyContext.RuntimeFileByName) {
                System.Console.Out.WriteLine($"  .  {runtimeFileByName.Key} {runtimeFileByName.Value.BaseFolder} {runtimeFileByName.Value.Value.Path}");
            }
        }

        var assemblyResolver = new AssemblyResolver(
            resolver: new CompositeCompilationAssemblyResolver(
            new ICompilationAssemblyResolver[]
            {
                latestAssemblyResolver,
                new AppBaseCompilationAssemblyResolver(),
                new ReferenceAssemblyPathResolver(),
                new PackageCompilationAssemblyResolver()
            }), plugInDependencyContext: plugInDependencyContext);

        var listTypePlugInModule = new List<System.Type>();
        foreach (var absolutePluginFilename in listAbsolutePluginFilename) {
            var assembly = assemblyResolver.AssemblyLoadContext.LoadFromAssemblyPath(absolutePluginFilename);
            foreach (var type in assembly.ExportedTypes) {
                if ("PlugInModule" == type.Name) {
                    listTypePlugInModule.Add(type);
                }
            }
        }
*/
