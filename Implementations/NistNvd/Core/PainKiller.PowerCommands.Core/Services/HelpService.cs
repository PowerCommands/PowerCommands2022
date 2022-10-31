using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Services;

public class HelpService : IHelpService
{
    private const bool WriteToLog = false;
    private HelpService(){}
    private static readonly Lazy<IHelpService> Lazy = new(() => new HelpService());
    public static IHelpService Service => Lazy.Value;
    public void ShowHelp(IConsoleCommand command, bool clearConsole = true)
    {
        var da = command.GetPowerCommandAttribute();
        if(clearConsole) Console.Clear();
        ConsoleService.WriteObjectDescription($"{GetType().Name}", "PowerCommand", command.Identifier, WriteToLog);
        var typeDescription = command.GetType().FullName!.StartsWith("PainKiller.PowerCommands.Core.Commands") ? "Core command (reserved name, not for use as custom command name)" : "Custom command";
        ConsoleService.WriteObjectDescription($"{GetType().Name}", "Type        ", $"{ typeDescription }", WriteToLog);
        ConsoleService.WriteObjectDescription($"{GetType().Name}", "Full name   ", $"{command.GetType().FullName}", WriteToLog);
        Console.WriteLine($"{da.Description}", WriteToLog);

        var args = da.Arguments.Split('|');
        var qutes = da.Qutes.Split('|');
        var flags = da.Flags.Split('|');
        var examples = da.Examples.Split('|');

        ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Arguments)}:",writeLog: WriteToLog);
        foreach (var a in args) WriteItem(a);
        ConsoleService.WriteObjectDescription($"{GetType().Name}", "Mandatory", da.ArgumentMandatory ? "Yes" : "No", WriteToLog);
        ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Qutes)}:",writeLog: WriteToLog);
        foreach (var q in qutes) WriteItem(q);
        ConsoleService.WriteObjectDescription($"{GetType().Name}", "Mandatory", da.QutesMandatory ? "Yes" : "No", WriteToLog);
        ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Flags)}:");
        foreach (var f in flags) WriteItem(f);

        if (!string.IsNullOrEmpty(da.Suggestion))
        {
            ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Suggestion)}:", writeLog: WriteToLog);
            ConsoleService.WriteLine(GetType().Name, da.Suggestion, null, writeLog: WriteToLog);
            Console.WriteLine();
        }
        Console.WriteLine();
        ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Examples)}:", writeLog: WriteToLog);
        foreach (var e in examples) WriteItem(e);
    }

    private void WriteItem(string description)
    {
        if (description.StartsWith("//"))
        {
            ConsoleService.WriteHeaderLine($"{GetType().Name}", $"\n{description}", ConsoleColor.Green);
            return;
        }
        ConsoleService.WriteLine(GetType().Name, description, null, WriteToLog);
    }
}