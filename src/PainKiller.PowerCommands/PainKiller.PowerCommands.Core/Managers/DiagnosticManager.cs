using System.Diagnostics;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Managers;

public class DiagnosticManager : IDiagnosticManager
{    
    private readonly Stopwatch _stopWatch = new();

    public DiagnosticManager(bool showDiagnostic)
    {
        ShowDiagnostic = showDiagnostic;
    }
    public bool ShowDiagnostic { get; set; }

    public void Message(string diagnostic)
    {
        if (!ShowDiagnostic) return;
        Console.WriteLine(diagnostic);
    }
    public void Start()
    {
        if (!ShowDiagnostic) return;
        _stopWatch.Start();
    }
    public void Stop()
    {
        if (!ShowDiagnostic) return;
        var ts = _stopWatch.Elapsed;
        _stopWatch.Reset();
        var elapsedTime = $"{ts.Hours:00}hh:{ts.Minutes:00}mm:{ts.Seconds:00}ss.{ts.Milliseconds / 10:00}ms";
        Console.WriteLine($"RunTime:{elapsedTime}");
    }
}