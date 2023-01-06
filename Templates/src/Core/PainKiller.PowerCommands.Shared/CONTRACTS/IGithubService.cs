namespace $safeprojectname$.Contracts;

public interface IGithubService
{
    void MergeDocsDB();
    void DownloadCommand(string commandName);
}