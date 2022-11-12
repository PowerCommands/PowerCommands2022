using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PowerCommands.WebShared.Models;

public class CommandMetadata
{
    public string Identifier { get; set; } = "";
    public List<string> Parameters { get; } = new();
    public List<PowerFlag> Flags { get; } = new();
    public List<string> Examples { get; } = new();
}