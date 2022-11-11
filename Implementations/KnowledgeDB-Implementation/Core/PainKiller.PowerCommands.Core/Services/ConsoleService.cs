using Microsoft.Extensions.Logging;

namespace PainKiller.PowerCommands.Core.Services;
public static class ConsoleService
{
    private static bool _disableLog;
    public static bool DisableLog
    {
        get => _disableLog;
        set
        {
            if(value) WriteWarning(nameof(ConsoleService),"Log from ConsoleService is disabled");
            else WriteLine(nameof(ConsoleService), "Log from ConsoleService is enabled", null);
            _disableLog = value;
        }
    }
    public static void WriteObjectDescription(string scope, string name, string description, bool writeLog = true)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{name}: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{description}");
        Console.ForegroundColor = currentColor;
        if(writeLog) WriteToLog(scope, $"{name} {description}");
    }
    public static void Write(string scope, string text, ConsoleColor? color = null, bool writeLog = true)
    {
        var currentColor = Console.ForegroundColor;
        if (color != null) Console.ForegroundColor = color.Value;
        Console.Write(text);
        Console.ForegroundColor = currentColor;
        if (writeLog) WriteToLog(scope, $"{text}");
    }

    public static void WriteLine(string scope, string text, ConsoleColor? color = null, bool writeLog = true)
    {
        var currentColor = Console.ForegroundColor;
        if(color != null) Console.ForegroundColor = color.Value;
        Console.WriteLine(text);
        Console.ForegroundColor = currentColor;
        if (writeLog) WriteToLog(scope, $"{text}");
    }
    public static void WriteHeaderLine(string scope, string text, ConsoleColor color = ConsoleColor.DarkCyan, bool writeLog = true)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = currentColor;
        if (writeLog) WriteToLog(scope, $"{text}");
    }
    public static void WriteWarning(string scope, string text)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(text);
        Console.ForegroundColor = currentColor;
        WriteToLog(scope, $"{text}", LogLevel.Warning);
    }
    public static void WriteError(string scope, string text)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ForegroundColor = currentColor;
        WriteToLog(scope, $"{text}", LogLevel.Error);
    }
    public static void WriteCritical(string scope, string text)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(text);
        Console.ForegroundColor = currentColor;
        WriteToLog(scope, $"{text}", LogLevel.Critical);
    }
    private static void WriteToLog(string scope, string message, LogLevel level = LogLevel.Information)
    {
        if(DisableLog) return;
        var text = $"{scope} {message}";
        switch (level)
        {
            case LogLevel.Information:
                IPowerCommandServices.DefaultInstance?.Logger.LogInformation(text);
                break;
            case LogLevel.Warning:
                IPowerCommandServices.DefaultInstance?.Logger.LogWarning(text);
                break;
            case LogLevel.Error:
                IPowerCommandServices.DefaultInstance?.Logger.LogError(text);
                break;
            case LogLevel.Critical:
                IPowerCommandServices.DefaultInstance?.Logger.LogCritical(text);
                break;
            case LogLevel.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }
    }
}