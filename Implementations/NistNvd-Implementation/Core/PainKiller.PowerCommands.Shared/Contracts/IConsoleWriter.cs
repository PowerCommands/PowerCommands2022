namespace PainKiller.PowerCommands.Shared.Contracts;
public interface IConsoleWriter
{
    void Write(string output, ConsoleColor? color = null);
    void WriteSuccess(string output);
    void WriteFailure(string output);
}