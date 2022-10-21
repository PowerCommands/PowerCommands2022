namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IDiagnosticManager
{
    bool ShowDiagnostic { get; set; }
    void Message(string diagnostic);
    void Start();
    void Stop();
    string RootPath();
}