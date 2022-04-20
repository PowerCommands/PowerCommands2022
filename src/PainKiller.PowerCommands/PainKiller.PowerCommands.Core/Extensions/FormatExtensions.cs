using static System.DateTime;
namespace PainKiller.PowerCommands.Core.Extensions;


public static class FormatExtensions
{
    public static string FormatFileTimestamp(this string prefix) => $"{prefix}{Now.Year}{Now.Month}{Now.Day}{Now.Hour}{Now.Minute}";
}