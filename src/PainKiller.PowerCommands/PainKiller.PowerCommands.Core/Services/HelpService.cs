using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Services;

public class HelpService : IHelpService
{
    private HelpService(){}
    private static readonly Lazy<IHelpService> Lazy = new(() => new HelpService());
    public static IHelpService Service => Lazy.Value;
    public void ShowHelp(IConsoleCommand command)
    {
        var da = command.GetPowerCommandAttribute();
        Console.WriteLine($"{nameof(da.Description)}:");
        Console.WriteLine(da.Description);
        Console.WriteLine();

        Console.WriteLine($"{nameof(da.DefaultParameter)}:");
        Console.WriteLine(da.DefaultParameter);
        Console.WriteLine();

        var args = da.Arguments.Split('|');
        var qutes = da.Qutes.Split('|');
        var examples = da.Examples.Split('|');

        Console.WriteLine($"{nameof(da.Arguments)}:");
        foreach (var a in args) Console.WriteLine(a);
        Console.WriteLine();
        Console.WriteLine($"{nameof(da.Qutes)}:");
        foreach (var q in qutes) Console.WriteLine(q);
        Console.WriteLine();
        Console.WriteLine($"{nameof(da.Examples)}:");
        foreach (var e in examples) Console.WriteLine(e);
    }
}