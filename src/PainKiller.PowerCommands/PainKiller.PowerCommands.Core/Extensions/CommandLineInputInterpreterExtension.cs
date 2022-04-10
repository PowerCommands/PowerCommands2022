using System.Text.RegularExpressions;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using static System.String;

namespace PainKiller.PowerCommands.Core.Extensions;

public static class CommandLineInputInterpreterExtension
{
    public static CommandLineInput Interpret(this string commandLineInput)
    {
        if(IsNullOrEmpty(commandLineInput)) throw new ArgumentNullException(nameof(commandLineInput));
        var raw = commandLineInput;
        var quotes = Regex.Matches(raw, "\\\"(.*?)\\\"").ToStringArray();
        var arguments = raw.RemoveQuotes(quotes).Split(' ').ToList();
        var identifier = $"{arguments[0].ToLower()}";
        arguments.RemoveAt(0);  //Remove identifier from arguments

        var retVal = new CommandLineInput {Arguments = arguments.ToArray(), Identifier = identifier, Quotes = quotes, Raw = raw};
        return retVal;
    }
    private static string RemoveQuotes(this string source, IEnumerable<string> matches)
    {
        var retVal = source;
        foreach (var match in matches) retVal = retVal.Replace($"\"{match}\"", "");
        return retVal;
    }
    private static string[] ToStringArray(this MatchCollection matches)
    {
        var retVal = new List<string>();
        foreach (Match match in matches) retVal.Add(match.ToString());
        return retVal.ToArray();
    }
}