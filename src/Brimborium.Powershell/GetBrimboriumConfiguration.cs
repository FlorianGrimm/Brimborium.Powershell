using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Brimborium.Powershell;
[Cmdlet(VerbsCommon.Get, "BrimboriumConfiguration")]
public class GetBrimboriumConfiguration :PSCmdlet {
    public string? Key { get; set; } = null;
    public Type? Type { get; set; } = null;
    public object? DefaultValue { get; set; } = null;
    protected override void BeginProcessing() {
    }
}
