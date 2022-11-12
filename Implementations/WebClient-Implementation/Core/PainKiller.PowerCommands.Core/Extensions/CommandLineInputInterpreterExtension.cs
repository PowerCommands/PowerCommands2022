﻿using System.Text.RegularExpressions;
using static System.String;

namespace PainKiller.PowerCommands.Core.Extensions;

public static class CommandLineInputInterpreterExtension
{
    public static CommandLineInput Interpret(this string commandLineInput, string defaultCommand = "commands")
    {
        if(IsNullOrEmpty(commandLineInput)) throw new ArgumentNullException(nameof(commandLineInput));
        var raw = commandLineInput.Trim();
        var quotes = Regex.Matches(raw, "\\\"(.*?)\\\"").ToStringArray();
        var arguments = raw.Split(' ').Where(r => !r.Contains('\"') && !r.StartsWith("--")).ToList();
        var flags = raw.Split(' ').Where(r => !r.Contains('\"') && r.StartsWith("--")).ToArray();
        var identifier = arguments.Count == 0 ? defaultCommand : $"{arguments[0].ToLower()}";
        if(arguments.Count > 0) arguments.RemoveAt(0);  //Remove identifier from arguments

        var retVal = new CommandLineInput {Arguments = arguments.ToArray(), Identifier = identifier, Quotes = quotes, Flags = flags, Raw = raw, Path = arguments.ToArray().ToPath()};
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
    public static string GetFlagValue(this ICommandLineInput input, string[] flags)
    {
        foreach (var inputFlag in input.Flags) if (flags.Any(f => $"--{f}" == inputFlag)) return inputFlag.Replace("--", "");
        return "";
    }
    public static string GetFlagValue(this ICommandLineInput input, string flagName)
    {
        var flag = input.Flags.FirstOrDefault(f => f == $"--{flagName.ToLower()}" || f.ToLower().Substring(2,1)  == $"{flagName.ToLower()}".Substring(0,1));
        if (IsNullOrEmpty(flag)) return "";

        var firstQuotedFlagValueIfAny = FindFirstQuotedFlagValueIfAny(input, flagName);
        if (!string.IsNullOrEmpty(firstQuotedFlagValueIfAny)) return firstQuotedFlagValueIfAny;

        short index = 0;
        var indexedInputs = input.Raw.Split(' ').Select(r => new IndexedInput{Index = index+=1,Value = r}).ToList();
        var flagIndex = indexedInputs.First(i => i.Value.ToLower() == flag.ToLower()).Index;
        var retVal = flagIndex == indexedInputs.Count ? "" : indexedInputs.First(i => i.Index == flagIndex + 1).Value.Replace("\"","");
        return retVal.StartsWith("--") ? "" : retVal;   //A flag could not have a flag as it´s value
    }
    public static bool HasFlag(this ICommandLineInput input, string flagName) => input.Flags.Any(f => f == $"--{flagName}");
    public static bool NoFlag(this ICommandLineInput input, string flagName) => !HasFlag(input, flagName);
    public static bool MustOnOfTheseFlagsCheck(this ICommandLineInput input, string[] flagNames) => flagNames.Any(flagName => flagName.ToLower() == flagName);
    public static void DoBadFlagCheck(this ICommandLineInput input, IConsoleCommand command)
    {
        var dokumentedFlags = command.GetPowerCommandAttribute().Flags.Split('|');
        foreach (var flag in input.Flags) if(dokumentedFlags.All(f => $"--{f.ToLower().Replace("!","")}" != flag.ToLower())) ConsoleService.Service.WriteLine($"{input.Identifier}", $"Warning, flag [{flag}] is not declared and probably unhandled in command [{command.Identifier}]", ConsoleColor.DarkYellow);
    }
    private static string FindFirstQuotedFlagValueIfAny(ICommandLineInput input, string flagName)
    {
        if (input.Quotes.Length == 0) return "";
        //First lets find out if the next parameter after the flag is a surrounded by " characters
        var lastIndexOfFlag = input.Raw.LastIndexOf($"--{flagName}") + $"--{flagName}".Length;
        var quotesAfterFlag = input.Quotes.Where(q => input.Raw.LastIndexOf(q) > lastIndexOfFlag).Select(q => new{Index = input.Raw.LastIndexOf(q), Quote = q }).ToList();
        var firstQuoteAfterFlag = quotesAfterFlag.FirstOrDefault(q => q.Index == quotesAfterFlag.Min(q => q.Index));
        if (firstQuoteAfterFlag == null) return "";
        var diff = firstQuoteAfterFlag.Index - lastIndexOfFlag;
        return diff > 1 ? "" : firstQuoteAfterFlag.Quote.Replace("\"","");
    }
}