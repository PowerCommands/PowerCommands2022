using System.Security.Cryptography;

namespace PainKiller.PowerCommands.Security.Managers;

internal static class ChecksumManager
{
    internal static string CalculateMd5(string filename)
    {
        if (!File.Exists(filename)) return "file missing...";
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filename);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}