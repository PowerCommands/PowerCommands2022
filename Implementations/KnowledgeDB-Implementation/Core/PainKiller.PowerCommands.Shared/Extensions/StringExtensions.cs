namespace PainKiller.PowerCommands.Shared.Extensions;

public static class StringExtensions
{
    public static string ToFirstLetterUpper(this string input)
    {
        if (input.Length < 2) return input.ToUpper();
        var firstLetter = input.Substring(0, 1).ToUpper();
        var retVal = $"{firstLetter}{input.Substring(1, input.Length - 1).ToLower()}";
        return retVal;
    }
    public static string ToFlagDescription(this string flag)
    {
        var required = flag.StartsWith('!') ? " (required)" : "";
        return string.IsNullOrEmpty(flag) ? "" : $"--{flag.Replace("!", "")}{required}";
    }
}