namespace PainKiller.PowerCommands.Configuration.Extensions;

public static class ConfigurationExtension
{
    public static string GetSafePathRegardlessHowApplicationStarted(this string fileName, string directory = "") => string.IsNullOrEmpty(directory) ? Path.Combine(AppContext.BaseDirectory, fileName) : Path.Combine(AppContext.BaseDirectory, directory, fileName);
}