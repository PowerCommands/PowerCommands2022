namespace PainKiller.PowerCommands.KnowledgeDBCommands.Extensions;

public static class FormatExtensions
{
    public static string ToSourceType(this ICommandLineInput input) => input.GetFlagValue(new[] { "onenote","url","path","file" });
}