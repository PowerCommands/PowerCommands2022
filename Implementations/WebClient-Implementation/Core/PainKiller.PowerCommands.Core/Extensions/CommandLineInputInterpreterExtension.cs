using System.Text.RegularExpressions;
using static System.String;

namespace PainKiller.PowerCommands.Core.Extensions;

public static class CommandLineInputInterpreterExtension
{
    public static ICommandLineInput Interpret(this string commandLineInput, string defaultCommand = "commands")
    {
        if(IsNullOrEmpty(commandLineInput)) throw new ArgumentNullException(nameof(commandLineInput));
        var raw = commandLineInput.Trim();
        var quotes = Regex.Matches(raw, "\\\"(.*?)\\\"").ToStringArray();
        var arguments = raw.Split(' ').Where(r => !r.Contains('\"') && !r.StartsWith("--")).ToList();
        var options = raw.Split(' ').Where(r => !r.Contains('\"') && r.StartsWith("--")).ToArray();
        var identifier = arguments.Count == 0 ? defaultCommand : $"{arguments[0].ToLower()}";
        if(arguments.Count > 0) arguments.RemoveAt(0);  //Remove identifier from arguments

        var retVal = new CommandLineInput {Arguments = arguments.ToArray(), Identifier = identifier, Quotes = quotes, Options = options, Raw = raw, Path = arguments.ToArray().ToPath()};
        return retVal;
    }
    public static string ToPath(this string[] inputArray)
    {
        if (inputArray.Length < 1) return "";
        var path = Join(' ', inputArray);
        return path;
    }
    private static string[] ToStringArray(this MatchCollection matches)
    {
        var retVal = new List<string>();
        foreach (Match match in matches) retVal.Add(match.ToString());
        return retVal.ToArray();
    }
    public static int FirstArgumentToInt(this ICommandLineInput input) => (int.TryParse(input.SingleArgument, out var index) ? index : 0);
    public static int OptionToInt(this ICommandLineInput input, string optionName, int valueIfMissing = 0) => (int.TryParse(GetOptionValue(input, optionName), out var index) ? index : valueIfMissing);
    public static string GetOptionValue(this ICommandLineInput input, string[] options)
    {
        foreach (var inputOption in input.Options) if (options.Any(f => $"--{f}" == inputOption)) return inputOption.Replace("--", "");
        return "";
    }
    public static string GetOptionValue(this ICommandLineInput input, string optionName)
    {
        var option = input.Options.FirstOrDefault(f => f == $"--{optionName.ToLower()}" || f.ToLower().Substring(2,1)  == $"{optionName.ToLower()}".Substring(0,1));
        if (IsNullOrEmpty(option)) return "";

        var firstQuotedOptionValueIfAny = FindFirstQuotedOptionValueIfAny(input, optionName);
        if (!string.IsNullOrEmpty(firstQuotedOptionValueIfAny)) return firstQuotedOptionValueIfAny;

        short index = 0;
        var indexedInputs = input.Raw.Split(' ').Select(r => new IndexedInput{Index = index+=1,Value = r}).ToList();
        var optionIndex = indexedInputs.First(i => i.Value.ToLower() == option.ToLower()).Index;
        var retVal = optionIndex == indexedInputs.Count ? "" : indexedInputs.First(i => i.Index == optionIndex + 1).Value.Replace("\"","");
        return retVal.StartsWith("--") ? "" : retVal;   //A option could not have a option as it´s value
    }
    public static bool HasOption(this ICommandLineInput input, string optionName) => input.Options.Any(f => f == $"--{optionName}");
    public static bool NoOption(this ICommandLineInput input, string optionName) => !HasOption(input, optionName);
    public static bool MustHaveOneOfTheseOptionCheck(this ICommandLineInput input, string[] optionNames) => optionNames.Any(optionName => optionName.ToLower() == optionName);
    public static string FirstOptionWithValue(this ICommandLineInput input) => $"{input.Options.FirstOrDefault(o => !IsNullOrEmpty(o))}".Replace("!", "").Replace("--", "");
    public static void DoBadOptionCheck(this ICommandLineInput input, IConsoleCommand command)
    {
        var dokumentedOptions = command.GetPowerCommandAttribute().Options.Split('|');
        foreach (var option in input.Options) if(dokumentedOptions.All(f => $"--{f.ToLower().Replace("!","")}" != option.ToLower())) ConsoleService.Service.WriteLine($"{input.Identifier}", $"Warning, option  [{option}] is not declared and probably unhandled in command [{command.Identifier}]", ConsoleColor.DarkYellow);
    }
    private static string FindFirstQuotedOptionValueIfAny(ICommandLineInput input, string optionName)
    {
        if (input.Quotes.Length == 0) return "";
        //First lets find out if the next parameter after the option is a surrounded by " characters
        var lastIndexOfOption = input.Raw.LastIndexOf($"--{optionName}") + $"--{optionName}".Length;
        var quotesAfterOption = input.Quotes.Where(q => input.Raw.LastIndexOf(q) > lastIndexOfOption).Select(q => new{Index = input.Raw.LastIndexOf(q), Quote = q }).ToList();
        var firstQuoteAfterOption = quotesAfterOption.FirstOrDefault(q => q.Index == quotesAfterOption.Min(q => q.Index));
        if (firstQuoteAfterOption == null) return "";
        var diff = firstQuoteAfterOption.Index - lastIndexOfOption;
        return diff > 1 ? "" : firstQuoteAfterOption.Quote.Replace("\"","");
    }
    public static string GetOutputFilename(this IConsoleCommand command) => Path.Combine(ConfigurationGlobals.ApplicationDataFolder, $"proxy_{command.Identifier}.data");
}