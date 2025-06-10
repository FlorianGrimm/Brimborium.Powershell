using System.Management.Automation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brimborium.Powershell;

[Cmdlet(VerbsCommon.Clear, "BrimboriumPowershell")]
public class ClearBrimboriumPowershell : PSCmdlet {
    [Parameter]
    public SwitchParameter FileLogger { get; set; }

    [Parameter]
    public SwitchParameter CustomSessionState { get; set; }

    protected override void EndProcessing() {
        var customSessionStateIsPresent = this.CustomSessionState.IsPresent;
        var fileLoggerIsPresent = this.FileLogger.IsPresent;
        var noneIsPresent = !customSessionStateIsPresent && !fileLoggerIsPresent;
        var customSessionStateValue = this.CustomSessionState.ToBool();
        var fileLoggerValue = this.FileLogger.ToBool();
        
        if (noneIsPresent || fileLoggerValue) {
            if (this.SessionState.PSVariable.GetValue("BrimboriumPowershellSessionState") is BrimboriumPowershellSessionState bpss) {
                var logger = bpss.ServiceProvider.GetRequiredService<ILogger<ClearBrimboriumPowershell>>();
                logger.LogDebug("Flush");
                bpss.FlushFileLogger();
            }
        }

        if (customSessionStateValue) {
            if (this.SessionState.PSVariable.GetValue("BrimboriumPowershellSessionState") is BrimboriumPowershellSessionState bpss) {
                bpss.Dispose();
            }
            this.SessionState.PSVariable.Remove("BrimboriumPowershellSessionState");
        }
    }
}
