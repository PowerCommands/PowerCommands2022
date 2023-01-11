namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IWorkingDirectoryChangesListener
{
    void OnWorkingDirectoryChanged(string workingDirectory);
    void InitializeWorkingDirectory();
}