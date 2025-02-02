using System.Globalization;

namespace PainKiller.PowerCommands.Core.Extensions
{
    public static class FileHandlingExtensions
    {
        public static int CopyFiles(this DirectoryInfo directoryInfo, string toPath)
        {
            var fileNames = directoryInfo.GetFiles();
            var fileCount = fileNames.Length;
            Directory.CreateDirectory(toPath);
            foreach (var fileInfo in fileNames) File.Copy(fileInfo.FullName, $"{toPath}\\{fileInfo.Name}");
            return fileCount;
        }
        public static ProxyResult GetLatestOutput(this string identifier)
        {
            var fileName = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, $"proxy_{identifier}.data");
            return StorageService<ProxyResult>.Service.GetObject(fileName);
        }
        public static string GetGlobalTranslationsFileName(this ITranslations translations) => Path.Combine(AppContext.BaseDirectory, $"gloooobal_translation_{CultureInfo.CurrentUICulture}.yaml");
    }
}