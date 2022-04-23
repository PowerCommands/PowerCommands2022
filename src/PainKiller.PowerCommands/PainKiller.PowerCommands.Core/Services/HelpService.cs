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
        Console.Clear();
        command.WriteObjectDescription("PowerCommand", command.Identifier);
        Console.WriteLine();
        command.WriteObjectDescription("Type        ", $"{command.GetType().FullName}");
        Console.WriteLine();
        Console.WriteLine($"{da.Description}");

        var args = da.Arguments.Split('|');
        var qutes = da.Qutes.Split('|');
        var examples = da.Examples.Split('|');

        command.WriteHeaderLine($"{nameof(da.Arguments)}:");
        foreach (var a in args) Console.WriteLine(a);
        Console.WriteLine();
        command.WriteHeaderLine($"{nameof(da.Qutes)}:");
        foreach (var q in qutes) Console.WriteLine(q);
        Console.WriteLine();
        if (!string.IsNullOrEmpty(da.DefaultParameter))
        {
            command.WriteHeaderLine($"{nameof(da.DefaultParameter)}:");
            Console.WriteLine(da.DefaultParameter);
            Console.WriteLine();
        }
        command.WriteHeaderLine($"{nameof(da.Examples)}:");
        foreach (var e in examples) Console.WriteLine(e);
    }
}