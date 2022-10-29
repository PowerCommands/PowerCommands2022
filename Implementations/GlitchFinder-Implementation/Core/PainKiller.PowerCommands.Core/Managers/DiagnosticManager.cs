using System.Diagnostics;
using System.Reflection;
using PainKiller.PowerCommands.Core.Services;
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
        ConsoleService.WriteLine(GetType().Name, diagnostic, null);
    }

    public void Header(string header)
    {
        if (!ShowDiagnostic) return;
        ConsoleService.WriteHeaderLine(GetType().Name, header);
    }

    public void Warning(string warning)
    {
        if (!ShowDiagnostic) return;
        ConsoleService.WriteWarning(GetType().Name, warning);
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
    public string RootPath()
    {
        var retVal = Assembly.GetEntryAssembly()?.Location ?? "";
        retVal = retVal.Replace($"{Assembly.GetEntryAssembly()?.GetName(false).Name}.dll", "");
        return retVal;
    }
}