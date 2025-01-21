using $safeprojectname$.DomainObjects.Core;

namespace $safeprojectname$.Contracts;

public interface IZipService
{
    ZipResult ArchiveFilesInDirectory(string directoryPath, string archiveName, bool useTimestampSuffix = false, string filter = "*", string outputDirectory = "");
    bool ExtractZipFile(string zipFilePath, string extractToDirectory);
}