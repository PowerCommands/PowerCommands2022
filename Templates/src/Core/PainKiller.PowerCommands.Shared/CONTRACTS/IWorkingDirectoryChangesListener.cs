namespace $safeprojectname$.Contracts;

public interface IWorkingDirectoryChangesListener
{
    void OnWorkingDirectoryChanged(string workingDirectory);
    void InitializeWorkingDirectory();
}