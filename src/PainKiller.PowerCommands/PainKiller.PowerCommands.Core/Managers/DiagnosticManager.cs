using System.Diagnostics;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Managers;

public class DiagnosticManager : IDiagnosticManager
{
    private readonly bool _showDiagnostic;
    private readonly Stopwatch _stopWatch = new();
    public DiagnosticManager(bool showDiagnostic)
    {
        _showDiagnostic = showDiagnostic;
    }
    public void Message(string diagnostic)
    {
        if (!_showDiagnostic) return;
        Console.WriteLine(diagnostic);
    }
    public void Start()
    {
        if (!_showDiagnostic) return;
        _stopWatch.Start();
    }
    public void Stop()
    {
        if (!_showDiagnostic) return;
        var ts = _stopWatch.Elapsed;
        _stopWatch.Reset();
        var elapsedTime = $"{ts.Hours:00}hh:{ts.Minutes:00}mm:{ts.Seconds:00}ss.{ts.Milliseconds / 10:00}ms";
        Console.WriteLine($"RunTime:{elapsedTime}");
    }
}