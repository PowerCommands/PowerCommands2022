namespace $safeprojectname$.Contracts;

public interface IInfoPanelManager
{
    void StartInfoPanelAsync();
    void StopUpdateReservedAreaAsync();
    void Display();
}