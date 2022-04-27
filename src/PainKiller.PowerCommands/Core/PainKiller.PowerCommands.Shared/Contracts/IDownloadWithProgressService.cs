namespace PainKiller.PowerCommands.Shared.Contracts;
public interface IDownloadWithProgressService
{
    Task Download(string downloadUrl, string destinationFilePath, Func<long?, long, double?, bool> progressChanged);
}