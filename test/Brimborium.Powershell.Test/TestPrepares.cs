using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;


using VerifyTUnit;

namespace Brimborium.Powershell;

public partial class TestPrepares {

    [Test]
    public async Task Test000VerifyChecksRun() {
        await VerifyChecks.Run();

        await Assert.That(
            System.IO.File.Exists(
                System.IO.Path.Combine(
                    GetSolutionRoot(),
                    "Brimborium.Powershell.sln")
            )).IsTrue();

        await Assert.That(
            System.IO.Directory.Exists(
                GetDocPath())).IsTrue();
    }

    [Test]
    public async Task Test003PackageReferenceCopied() {
        var solutionRoot = GetSolutionRoot();
        var csprojPath = System.IO.Path.Combine(solutionRoot, "src", "Brimborium.Powershell", "Brimborium.Powershell.csproj");
        await Assert.That(System.IO.File.Exists(csprojPath)).IsTrue();
        var project = Microsoft.Build.Construction.ProjectRootElement.Open(csprojPath);

        var groupWithNoneCopy = project.ItemGroups
            .Where(
                group => group.Items.Any(
                    item => item.ItemType == "None"
                        && item.HasMetadata
                        && item.Metadata.Any(
                            metadata => metadata.Name == "CopyToOutputDirectory"
                                && metadata.Value == "PreserveNewest"))
            ).LastOrDefault();
        await Assert.That(groupWithNoneCopy).IsNotNull();

        var listPackageReferences =        project.ItemGroups
            .SelectMany(group => group.Items)
            .Where(item => item.ItemType == "PackageReference"
                && item.HasMetadata
                && item.Metadata.Any(
                    metadata => metadata.Name == "GeneratePathProperty"
                        && metadata.Value == "true"))
            .ToList();
        var listNoneWithCopy = project.ItemGroups
            .SelectMany(group => group.Items)
            .Where(item => item.ItemType == "None"
                && item.Include.StartsWith("$(Pkg")
                && item.HasMetadata
                && item.Metadata.Any(
                    metadata => metadata.Name == "CopyToOutputDirectory"
                        && metadata.Value == "PreserveNewest"));

        await Assert.That(listPackageReferences.Count).IsGreaterThan(0);

        foreach(var item in listPackageReferences) {
            var include = item.Include;
            var includeWithUnderscore = item.Include.Replace('.', '_');
            var found = listNoneWithCopy
                .Any(n => n.Include.Contains(includeWithUnderscore));
            await Assert.That(found).IsTrue().Because(
                $"""
                <None Include="$(Pkg{includeWithUnderscore})/lib/net9.0/{include}.dll" CopyToOutputDirectory="PreserveNewest"  >
                  <Link>{include}.dll</Link>
                  <Visible>false</Visible>
                </None>
                """);
        }
    }

    [Test]
    public async Task Test001GeneratePS1Tests() {
        var projectPath = GetProjectPath();
        var pathDocs = GetDocPath();
        StringBuilder sbChangedFilename = new();
        StringBuilder sbOutput = new();
        var listMDFilePath = System.IO.Directory.EnumerateFiles(pathDocs, "*.md")
            .OrderBy(fp => fp)
            .ToList();
        Regex regexInvalidChars = new("[^A-Za-z0-9 ]+");
        Regex regexUpperCase = new("[ ][a-z]");
        Regex regexSpace = new("[ ]+");
        Regex regexExample = new("### Example [0-9]+");
        Regex regexStartContent = new("[#][ ][<]Content[>]");
        Regex regexFinishContent = new("[#][ ][<][/]Content[>]");
        foreach (var mdFilePath in listMDFilePath) {
            var mdContent = System.IO.File.ReadAllLines(mdFilePath);
            var mdFileName = System.IO.Path.GetFileNameWithoutExtension(mdFilePath);
            int state = 0;
            string stateHeadline = "";
            string stateFilename = "";
            for (int iLine = 0; iLine < mdContent.Length; iLine++) {
                var line = mdContent[iLine];
                if (line.StartsWith("### Example ")) {
                    stateHeadline = line;
                    var match = regexExample.Match(line);
                    if (match.Success) {
                        stateFilename = match.Value;
                    } else {
                        stateFilename = line;
                    }
                    var txt = regexInvalidChars.Replace(stateFilename, " ");
                    txt = regexUpperCase.Replace(txt, (Match match) => match.Value.Trim().ToUpperInvariant());
                    stateFilename = regexSpace.Replace(txt, "");
                } else if ("```powershell" == line) {
                    state = 1;
                    sbOutput.Clear();
                    sbOutput.AppendLine("# <Content>");
                    sbOutput.AppendLine($"# {mdFileName}:{(iLine + 1)}");
                } else if ("```" == line) {
                    if (1 == state) {
                        state = 0;
                        sbOutput.AppendLine("#");
                        sbOutput.AppendLine("# </Content>");
                        var stateFilenameLimited = (stateFilename.Length <= 32) ? stateFilename : stateFilename.Substring(0, 32);
                        var outputFilename = $"Tests.{mdFileName}-{stateFilenameLimited}.ps1".Replace('-', '_');
                        var pathTest = System.IO.Path.Combine(projectPath, outputFilename);

                        string oldContent = string.Empty;
                        string oldContentStart =
                            """
                            Set-StrictMode -Version Latest

                            Import-Module 'Brimborium.Powershell'

                            #
                            """;
                        string oldContentFinish =
                            """
                            #
                            '- fini -'
                            """;

                        if (System.IO.File.Exists(pathTest)) {
                            oldContent = System.IO.File.ReadAllText(pathTest);
                            var matchStart = regexStartContent.Match(oldContent);
                            if (matchStart.Success) {
                                var matchFinish = regexFinishContent.Match(oldContent);
                                if (matchFinish.Success) {
                                    oldContentStart = oldContent.Substring(0, matchStart.Index).Trim() + System.Environment.NewLine;
                                    oldContentFinish = oldContent.Substring(matchFinish.Index + matchFinish.Length).Trim();
                                }
                            }
                        }
                        sbOutput.Insert(0, oldContentStart).Append(oldContentFinish);
                        var nextContent = sbOutput.ToString();
                        if (!string.Equals(oldContent, nextContent, StringComparison.Ordinal)) {
                            //Assert.Fail(nextContent);
                            System.IO.File.WriteAllText(pathTest, nextContent);
                            sbChangedFilename.AppendLine(outputFilename);
                        } else if ("" == oldContent) {
                            System.IO.File.WriteAllText(pathTest, nextContent);
                            sbChangedFilename.AppendLine(outputFilename);
                        }
                        sbOutput.Clear();
                        stateHeadline = stateFilename = "";
                    }
                } else if (1 == state) {
                    if (@"PS C:\> {{ Add example code here }}" == line) {
                        state = 0;
                        sbOutput.Clear();
                        stateHeadline = stateFilename = "";
                    } else {
                        sbOutput.AppendLine(line);
                    }
                }
            }
        }
        {
            var result = sbChangedFilename.ToString();
            if (string.IsNullOrEmpty(result)) { result = "NoChanges"; }
            await Verify(result);
        }
    }

    [Test]
    public async Task Test002GenerateCSTests() {
        var projectPath = GetProjectPath();
        var listTestPs1 = System.IO.Directory.EnumerateFiles(projectPath, "*.ps1")
            .Select(f => System.IO.Path.GetFileNameWithoutExtension(f))
            .Where(f => f != "TestInclude")
            .OrderBy(f => f)
            .Select(f => {
                if (f.StartsWith("Tests.")) { f = f.Substring(6); }
                return $"    [Test]\r\n    public async Task {f}() => await Verify(this.RunPowershellTest());\r\n\r\n";
            })
            .ToList();
        string result = string.Join("", listTestPs1);
        if (string.IsNullOrEmpty(result)) { result = "NoTests"; }
        await Verify(result);
    }

    private static string GetSolutionRoot([CallerFilePath] string? callerFilePath = default) {
        var result = callerFilePath ?? string.Empty;
        for (int i = 0; (i < 3) && !string.IsNullOrEmpty(result); i++) {
            result = System.IO.Path.GetDirectoryName(result);
        }
        return result ?? string.Empty;
    }

    private static string? _GetProjectPathCache;
    private static string GetProjectPath([CallerFilePath] string? callerFilePath = default) {
        if (_GetProjectPathCache is { } result) {
            return result;
        } else {
            result = System.IO.Path.GetDirectoryName(
                callerFilePath ?? throw new ArgumentException(nameof(callerFilePath))
                ) ?? throw new ArgumentException(nameof(callerFilePath));
            return _GetProjectPathCache = result;
        }
    }

    private static string GetDocPath() {
        var solutionRoot = GetSolutionRoot();
        var pathDocs = System.IO.Path.Combine(solutionRoot, "src", "Brimborium.Powershell", "docs");
        return pathDocs;
    }
}
