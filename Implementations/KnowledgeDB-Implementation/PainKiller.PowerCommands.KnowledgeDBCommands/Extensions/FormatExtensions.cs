namespace PainKiller.PowerCommands.KnowledgeDBCommands.Extensions;

public static class FormatExtensions
{
    public static string ToSourceType(this ICommandLineInput input) => input.GetOptionValue(new[] { "onenote","url","path","file" });
}