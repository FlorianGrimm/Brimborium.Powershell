# Brimborium.Powershell

```powershell
Import-Module -Name Brimborium.Powershell

Start-BrimboriumPowershell -ConfigurationPath .\appsettings.json
$value = Get-BrimboriumConfiguration -Key "abc:def" -Type [int] -Default 42
$logger = Get-BrimboriumLogger -Name "MyLogger"
Clear-BrimboriumPowershell


[System.AppDomain]::CurrentDomain.GetAssemblies()|%{$_.GetName()}|Sort-Object Name

```


