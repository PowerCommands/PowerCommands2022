namespace PainKiller.PowerCommands.Shared.Contracts;

public interface ICliManager
{
    void CreateRootDirectory();
    void CloneRepo(string repo);
    void DeleteDir(string directory);
    void MoveDir(string directory, string name);
    void WriteNewSolutionFile();
}