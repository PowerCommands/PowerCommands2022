using $safeprojectname$.Managers;

namespace $safeprojectname$.DomainObjects;

public class FileChecksum
{
    public string Mde5Hash { get; }
    public FileChecksum(string fileName) { Mde5Hash = ChecksumManager.CalculateMd5ForFile(fileName); }
}