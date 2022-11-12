namespace PainKiller.PowerCommands.Core.Services;

public interface IConsoleService
{
    bool DisableLog { get; set; }
    void WriteObjectDescription(string scope, string name, string description, bool writeLog = true);
    void Write(string scope, string text, ConsoleColor? color = null, bool writeLog = true);
    void WriteLine(string scope, string text, ConsoleColor? color = null, bool writeLog = true);
    void WriteHeaderLine(string scope, string text, ConsoleColor color = ConsoleColor.DarkCyan, bool writeLog = true);
    void WriteWarning(string scope, string text);
    void WriteError(string scope, string text);
    void WriteCritical(string scope, string text);
}