namespace PainKiller.PowerCommands.Shared.Contracts;

public interface ICliManager
{
    void CreateRootDirectory();
    void CreateDirectory(string name);
    void CloneRepo(string repo);
    void DeleteDir(string directory);
    void DeleteFile(string fileName, bool repoFile);
    void RenameDirectory(string directory, string name);
    void MoveDirectory(string dirctoryName, string toDirctoryName);
    void MoveFile(string fileName, string toFileName);
    void WriteNewSolutionFile();
    void ReplaceContentInFile(string fileName, string find, string replace);
}