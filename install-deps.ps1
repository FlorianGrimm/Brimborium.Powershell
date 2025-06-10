<#
pwsh -f .\install-deps.ps1
#>

<#
https://github.com/PowerShell/platyPS
#>
Install-Module -Name platyPS -Scope CurrentUser
Import-Module platyPS

<#
$m=import-module '.\Output\Brimborium.Powershell.psd1' -PassThru
$m|fl

New-MarkdownHelp -Module 'Brimborium.Powershell' -OutputFolder .\src\Brimborium.Powershell\docs
New-ExternalHelp .\src\Brimborium.Powershell\docs -OutputPath .\src\Brimborium.Powershell\help\

# re-import your module with latest changes
import-module '.\Output\Brimborium.Powershell.psd1'
Update-MarkdownHelp .\src\Brimborium.Powershell\docs
#>
