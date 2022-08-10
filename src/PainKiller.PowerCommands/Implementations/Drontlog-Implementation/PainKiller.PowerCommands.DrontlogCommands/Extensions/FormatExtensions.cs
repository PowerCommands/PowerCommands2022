namespace PainKiller.PowerCommands.DrontlogCommands.Extensions;

public static class FormatExtensions
{
    public static string ToDateString(this DateTime dateTime) => $"{dateTime.Year}-{dateTime.Month.ToString().PadLeft(2,'0')}-{dateTime.Day.ToString().PadLeft(2, '0')}";
}