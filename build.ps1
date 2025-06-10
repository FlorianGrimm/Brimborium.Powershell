param(
    [string]$Configuration = "Debug"
)

dotnet build --configuration $Configuration

<#
if ($Configuration -eq "Debug") {
    Import-Module platyPS
    import-module './Output/Brimborium.Powershell.psd1'
    # Update-MarkdownHelp ./src/Brimborium.Powershell/docs
    Update-MarkdownHelpModule -Path ./src/Brimborium.Powershell/docs
    # New-ExternalHelp ./src/Brimborium.Powershell/docs -OutputPath ./src/Brimborium.Powershell/ -Force
}
#>


if ($Configuration -eq "Debug") {
    Import-Module platyPS
    Import-Module './Output/Brimborium.Powershell.psd1'
    
    # Generate or update markdown help files
    if (-not (Test-Path ./src/Brimborium.Powershell/docs)) {
        New-MarkdownHelp -Module 'Brimborium.Powershell' -OutputFolder ./src/Brimborium.Powershell/docs -Force
    } else {
        Update-MarkdownHelpModule -Path ./src/Brimborium.Powershell/docs
    }
    
    # Generate external help files (XML)
    New-ExternalHelp -Path ./src/Brimborium.Powershell/docs -OutputPath ./src/Brimborium.Powershell/help -Force
}

if ($Configuration -eq "Debug") {
    dotnet test "test/Brimborium.Powershell.Test/Brimborium.Powershell.Test.csproj" --filter "FullyQualifiedName~Brimborium.Powershell.TestPrepares"
    dotnet test "test/Brimborium.Powershell.Test/Brimborium.Powershell.Test.csproj" --filter "FullyQualifiedName~Brimborium.Powershell.Tests"
}

if ($Configuration -eq "Release") {

    dotnet test --configuration Release

    dotnet publish src/Brimborium.Powershell/Brimborium.Powershell.csproj --configuration $Configuration --output Package

    mkdir ./assets -ErrorAction SilentlyContinue | out-null

    Compress-Archive -Path ./Package/ -DestinationPath "./assets/Brimborium.Powershell-$([System.DateTime]::Today.ToString('yyyy-mm-dd')).zip"
}
