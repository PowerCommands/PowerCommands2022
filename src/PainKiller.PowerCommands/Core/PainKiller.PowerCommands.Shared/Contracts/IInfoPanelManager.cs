namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IInfoPanelManager
{
    void StartInfoPanelAsync();
    void StopUpdateReservedAreaAsync();
    void Display();
}