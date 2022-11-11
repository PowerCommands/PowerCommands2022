using PainKiller.PowerCommands.Shared.Extensions;

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
        var examples = da.Examples.Split('|');

        ConsoleService.WriteHeaderLine($"{GetType().Name}", "\nDescription", writeLog: WriteToLog);
        Console.WriteLine($"{da.Description}", WriteToLog);
        Console.WriteLine();

        args = da.Arguments.Replace("!","").Split('|');
        quotes = da.Quotes.Replace("!", "").Split('|');
        var parameters = new List<string>();
        parameters.AddRange(args);
        parameters.AddRange(quotes);
        flags = da.Flags.Replace("!", "").Split('|');
        
        ConsoleService.WriteHeaderLine(nameof(HelpService), "Usage");

        var argsMarkup = args.Any(a => !string.IsNullOrEmpty(a)) ? "[arguments]" : "";
        var quotesMarkup = quotes.Any(q => !string.IsNullOrEmpty(q)) ? "[quotes]" : "";
        var flagsMarkup = flags.Any(f => !string.IsNullOrEmpty(f)) ? "[flags]" : "";

        ConsoleService.Write(nameof(HelpService), command.Identifier, ConsoleColor.Blue);
        Console.WriteLine($" {argsMarkup} {quotesMarkup} {flagsMarkup}");
        Console.WriteLine("");
        ConsoleService.WriteHeaderLine(nameof(HelpService),"Flag options:");
        var flagDescriptions = flags.Select(f => f.ToFlagDescription());
        Console.WriteLine( string.Join(',',flagDescriptions));
        Console.WriteLine("");
        if (string.IsNullOrEmpty(da.Examples)) return;
        
        ConsoleService.WriteHeaderLine($"{GetType().Name}", $"{nameof(da.Examples)} Commands:", writeLog: WriteToLog);
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