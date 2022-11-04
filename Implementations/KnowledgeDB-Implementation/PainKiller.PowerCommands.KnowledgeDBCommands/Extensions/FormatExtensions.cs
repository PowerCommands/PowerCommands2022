namespace PainKiller.PowerCommands.KnowledgeDBCommands.Extensions;

public static class FormatExtensions
{
    public static string ToSourceType(this ICommandLineInput input) => input.HasFlag("onenote") ? "onenote" : input.HasFlag("path") ? "path" : "url";
    public static string ToUri(this ICommandLineInput input) => input.HasFlag("onenote") ? input.GetFlagValue("onenote") : input.HasFlag("path") ? input.GetFlagValue("path") : input.GetFlagValue("url");
}