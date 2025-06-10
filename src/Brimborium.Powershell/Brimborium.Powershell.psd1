@{
    RootModule = 'Brimborium.Powershell.dll'
    ModuleVersion = '1.0.0'
    GUID = '24b80cb8-bcf5-4ada-a6dc-da61e7404168'
    Author = 'Florian Grimm'
    CompanyName = ''
    Copyright = 'MIT License.'
    # Description = ''
    # PowerShellVersion = ''
    # PowerShellHostName = ''
    # PowerShellHostVersion = ''
    # DotNetFrameworkVersion = ''
    # CLRVersion = ''
    # ProcessorArchitecture = ''

    RequiredModules = @()
    RequiredAssemblies = @(
    )
    ScriptsToProcess = @()
    TypesToProcess = @()
    FormatsToProcess = @()
    NestedModules = @()
    FunctionsToExport = @()
    CmdletsToExport = @(
        "Start-BrimboriumPowershell",
        "Clear-BrimboriumPowershell",
        "Get-BrimboriumConfiguration"
    )

    VariablesToExport = ''
    AliasesToExport = @()
    DscResourcesToExport = @()
    ModuleList = @()
    FileList = @()

    PrivateData = @{
        PSData = @{
            # 'Tags' wurde auf das Modul angewendet und unterstützt die Modulermittlung in Onlinekatalogen.
            # Tags = @()

            # Eine URL zur Lizenz für dieses Modul.
            # LicenseUri = ''

            # Eine URL zur Hauptwebsite für dieses Projekt.
            # ProjectUri = ''

            # Eine URL zu einem Symbol, das das Modul darstellt.
            # IconUri = ''

            # 'ReleaseNotes' des Moduls
            # ReleaseNotes = ''

        } # Ende der PSData-Hashtabelle
    } # Ende der PrivateData-Hashtabelle

    # HelpInfoURI = ''

    DefaultCommandPrefix = ''
}
