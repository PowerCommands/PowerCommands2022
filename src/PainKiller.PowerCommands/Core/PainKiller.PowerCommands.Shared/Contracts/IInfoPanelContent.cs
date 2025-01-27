namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IInfoPanelContent
{
    string GetText();
    string? ShortText { get; }
}