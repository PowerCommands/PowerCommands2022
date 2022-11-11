namespace PainKiller.PowerCommands.Shared.Contracts;
public interface IConsoleWriter
{
    void Write(string output, bool addToOutput = true, ConsoleColor? color = null);
    void WriteSuccess(string output, bool addToOutput = true);
    void WriteFailure(string output, bool addToOutput = true);
}