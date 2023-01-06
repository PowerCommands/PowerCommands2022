using $safeprojectname$.DomainObjects;

namespace $safeprojectname$.Extensions;

public static class SecurityExtensions
{
    public static bool CompareFileChecksum(this FileChecksum fileChecksum, string checksum) => fileChecksum.Mde5Hash == checksum;
}