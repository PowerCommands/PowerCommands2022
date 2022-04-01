namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;

public class CommandLineInput
{
    public string Raw { get; init; } = "";
    public string Identifier { get; init; } = "";
    public string[] Quotes { get; init; } = null!;
    public string[] Arguments { get; init; } = null!;
}