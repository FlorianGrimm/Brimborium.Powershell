---
external help file: Brimborium.Powershell.dll-Help.xml
Module Name: Brimborium.Powershell
online version:
schema: 2.0.0
---

# Clear-BrimboriumPowershell

## SYNOPSIS
Flushes the file logger and cleans up resources used by the Brimborium PowerShell environment.

## SYNTAX

```
Clear-BrimboriumPowershell [-FileLogger] [-CustomSessionState] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

## DESCRIPTION
The Clear-BrimboriumPowershell cmdlet properly disposes of resources that were initialized by Start-BrimboriumPowershell. This cmdlet performs cleanup operations such as:

- Flushing log buffers

This cmdlet should be called when you're done using the Brimborium PowerShell functionality to ensure proper cleanup and prevent resource leaks.

## EXAMPLES

### Example 1: Basic cleanup
```powershell
Clear-BrimboriumPowershell
```

This example shows how to clean up resources used by the Brimborium PowerShell environment.

### Example 2: Cleanup in a script
```powershell
try {
    Start-BrimboriumPowershell
    # Your script logic here
} finally {
    Clear-BrimboriumPowershell
}
```

This example shows how to use Clear-BrimboriumPowershell in a script to ensure cleanup even if errors occur.
Clear-BrimboriumPowershell -FileLogger is called automatically when the PowerShell session ends.

## PARAMETERS

### -CustomSessionState
{{ Fill CustomSessionState Description }}

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

### -FileLogger
{{ Fill FileLogger Description }}

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
{{ Fill ProgressAction Description }}

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

## OUTPUTS

## NOTES

## RELATED LINKS
