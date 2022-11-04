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

        var args = da.Arguments.Split('|');
        var quotes = da.Quotes.Split('|');
        var flags = da.Flags.Split('|');
        var secrets = da.Secrets.Split('|');
        var examples = da.Examples.Split('|');

        ConsoleService.WriteHeaderLine($"{GetType().Name}", "\nDescription", writeLog: WriteToLog);
        Console.WriteLine($"{da.Description}", WriteToLog);
        if (IPowerCommandServices.DefaultInstance!.Configuration.ShowDiagnosticInformation)
        {
            ConsoleService.WriteObjectDescription($"{GetType().Name}", "\nPowerCommand", command.Identifier, WriteToLog);
            var typeDescription = command.GetType().FullName!.StartsWith("PainKiller.PowerCommands.Core.Commands") ? "Core command (reserved name, not for use as custom command name)" : "Custom command";
            ConsoleService.WriteObjectDescription($"{GetType().Name}", "Type        ", $"{typeDescription}", WriteToLog);
            ConsoleService.WriteObjectDescription($"{GetType().Name}", "Full name   ", $"{command.GetType().FullName}", WriteToLog);
            ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Arguments)}:", writeLog: WriteToLog);
            foreach (var a in args) WriteItem(a.Replace("!","Required: "));
            ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Quotes)}:", writeLog: WriteToLog);
            foreach (var q in quotes) WriteItem(q.Replace("!", "Required: "));
            ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Flags)}:");
            foreach (var f in flags) WriteItem(f.Replace("!", "Requires a value: "));
            ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Secrets)}:");
            foreach (var s in flags) WriteItem(s.Replace("!", "Required: "));

            if (!string.IsNullOrEmpty(da.Suggestion))
            {
                ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Suggestion)}:", writeLog: WriteToLog);
                ConsoleService.WriteLine(GetType().Name, da.Suggestion, null, writeLog: WriteToLog);
                Console.WriteLine();
            }

        }
        args = da.Arguments.Replace("!","").Split('|');
        quotes = da.Quotes.Replace("!", "").Split('|');
        flags = da.Flags.Replace("!", "").Split('|');

        Console.WriteLine();
        ConsoleService.Write(nameof(HelpService), $"{command.Identifier}", ConsoleColor.Blue, WriteToLog);
        ConsoleService.WriteLine(nameof(HelpService), $"{ToDescription(args," ")}{ToDescription(quotes, " ")}{ToDescription(flags, " --")}", null, WriteToLog);
        Console.WriteLine();

        if (string.IsNullOrEmpty(da.Examples)) return;
        ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Examples)}:", writeLog: WriteToLog);
        foreach (var e in examples) WriteItem(e, command.Identifier);
    }
    private void WriteItem(string description, string identifier = "")
    {
        if (description.StartsWith("//"))
        {
            ConsoleService.WriteHeaderLine($"{GetType().Name}", $"\n{description}", ConsoleColor.Green);
            return;
        }
        ConsoleService.Write(GetType().Name, $"{identifier} ", ConsoleColor.Blue, WriteToLog);
        ConsoleService.WriteLine(GetType().Name, description.Replace(identifier,"").Trim(), null, WriteToLog);
    }
    private string ToDescription(string[] parameters, string separator) => parameters.Length == 0 || (parameters.Length == 1 && string.IsNullOrEmpty(parameters[0])) ? "" : $"{separator}{string.Join(separator, parameters)}";
}