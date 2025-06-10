---
external help file: Brimborium.Powershell.dll-Help.xml
Module Name: Brimborium.Powershell
online version:
schema: 2.0.0
---

# Start-BrimboriumPowershell

## SYNOPSIS
Initializes the Brimborium PowerShell environment with configuration settings.

## SYNTAX

```
Start-BrimboriumPowershell [-ConfigurationPath <String>] [-Configuration2Path <String>] [-EnableConsoleLogging]
 [-EnableFileLogging] [-PassThru] [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

## DESCRIPTION
The Start-BrimboriumPowershell cmdlet initializes the Brimborium PowerShell environment. It sets up the configuration system that can be used by other cmdlets in the module. This cmdlet should be called before using other Brimborium PowerShell functionality.

## EXAMPLES

### Example 1
```powershell
Start-BrimboriumPowershell
```

Initializes the Brimborium PowerShell environment with default settings.

### Example 2
```powershell
Start-BrimboriumPowershell -ConfigurationPath .\appsettings.json
```

Initializes the Brimborium PowerShell environment using the specified configuration file.

## PARAMETERS

### -Configuration2Path
{{ Fill Configuration2Path Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ConfigurationPath
{{ Fill ConfigurationPath Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EnableConsoleLogging
{{ Fill EnableConsoleLogging Description }}

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EnableFileLogging
{{ Fill EnableFileLogging Description }}

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
{{ Fill PassThru Description }}

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ProgressAction
Controls how progress reporting is handled during cmdlet execution.

```yaml
Type: ActionPreference
Parameter Sets: (All)
Aliases: proga

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object
## NOTES
After initializing the environment with Start-BrimboriumPowershell, you can use Get-BrimboriumConfiguration to retrieve configuration values and Get-BrimboriumLogger to obtain a logger instance.

## RELATED LINKS
