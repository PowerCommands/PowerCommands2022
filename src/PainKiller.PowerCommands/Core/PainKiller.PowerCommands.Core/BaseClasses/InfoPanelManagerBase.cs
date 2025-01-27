using System.Drawing;

namespace PainKiller.PowerCommands.Core.BaseClasses;

public abstract class InfoPanelManagerBase(InfoPanelConfiguration configuration, IInfoPanelContent panel1, IInfoPanelContent panel2, IInfoPanelContent panel3) : IDisposable, IInfoPanelManager
{
    private Task? _infoPanelTask;
    private CancellationTokenSource? _cts;
    private int _previousWidth = -1;

    public virtual void StartInfoPanelAsync()
    {
        _previousWidth = Console.WindowWidth;
        _cts = new CancellationTokenSource();
        _infoPanelTask = RunInfoPanelAsync(_cts.Token);
    }
    public virtual void StopUpdateReservedAreaAsync()
    {
        if (_cts == null) return;
        _cts.Cancel();
        _infoPanelTask?.Wait();
    }
    public virtual void Display() => Display(-1);
    private async Task RunInfoPanelAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                Display(_previousWidth);
                _previousWidth = Console.WindowWidth;
                await Task.Delay(configuration.UpdateIntervalSeconds * 1000, token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                ConsoleService.Service.WriteError(nameof(RunInfoPanelAsync), ex.Message);
            }
        }
    }
    protected virtual void Display(int previousWidth)
    {
        if (previousWidth == -1) previousWidth = Console.WindowWidth;
        
        var startLocation = new Point(Console.CursorLeft, Console.CursorTop);

        if ((previousWidth > Console.WindowWidth || Console.CursorTop < configuration.Height) && (configuration.AutoAdjustOnResize && RunFlowManager.CommandIsRunning == false))
        {
            ConsoleService.Service.Clear();
            ConsoleService.Service.WritePrompt();
            startLocation = new Point((int)IPowerCommandServices.DefaultInstance?.Configuration.Prompt.Length!, configuration.Height);
        }
        
        var backgroundColor = configuration.GetConsoleColor();
        var originalBackgroundColor = Console.BackgroundColor;
        
        Console.BackgroundColor = backgroundColor;

        Console.SetCursorPosition(0, 0);
        var row = panel1.GetText();
        var rowShort = panel1.ShortText;
        Console.Write(row.Length > Console.WindowWidth - 2 ? rowShort : row);
        Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));

        Console.SetCursorPosition(row.Length +1, 0);
        var row2 = panel2.GetText();
        var row2Short = panel2.ShortText;
        Console.Write(row2.Length > Console.WindowWidth - 2 ? row2Short : row2);
        Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));

        
        Console.SetCursorPosition(0, 1);
        var row3 = panel3.GetText();
        var row3Short = panel3.ShortText;
        Console.Write(row3.Length > Console.WindowWidth - 2 ? row3Short : row3);
        Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));

        Console.BackgroundColor = originalBackgroundColor;

        Console.SetCursorPosition(startLocation.X, startLocation.Y);
    }
    void IDisposable.Dispose()
    {
        _infoPanelTask?.Dispose();
        _cts?.Cancel();
        _cts?.Dispose();
    }
}