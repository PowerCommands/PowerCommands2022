using System.Text.RegularExpressions;
using static System.String;

namespace PainKiller.PowerCommands.Core.Extensions;

public static class CommandLineInputInterpreterExtension
{
    public static CommandLineInput Interpret(this string commandLineInput)
    {
        if(IsNullOrEmpty(commandLineInput)) throw new ArgumentNullException(nameof(commandLineInput));
        var raw = commandLineInput.Trim();
        var quotes = Regex.Matches(raw, "\\\"(.*?)\\\"").ToStringArray();
        var arguments = raw.Split(' ').Where(r => !r.Contains('\"')).ToList();
        var identifier = $"{arguments[0].ToLower()}";
        arguments.RemoveAt(0);  //Remove identifier from arguments

        var retVal = new CommandLineInput {Arguments = arguments.ToArray(), Identifier = identifier, Quotes = quotes, Raw = raw, Path = arguments.ToArray().ToPath()};
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
}