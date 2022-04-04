namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IDiagnosticManager
{
    void Message(string diagnostic);
    void Start();
    void Stop();
}