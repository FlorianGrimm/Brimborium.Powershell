namespace Brimborium.Powershell;

public partial class Tests {
    [Test]
    public async Task Clear_BrimboriumPowershell_Example1() => await Verify(this.RunPowershellTest());

    [Test]
    public async Task Clear_BrimboriumPowershell_Example2() => await Verify(this.RunPowershellTest());

    [Test]
    public async Task Start_BrimboriumPowershell_Example1() => await Verify(this.RunPowershellTest());

    [Test]
    public async Task Start_BrimboriumPowershell_Example2() => await Verify(this.RunPowershellTest());
}
