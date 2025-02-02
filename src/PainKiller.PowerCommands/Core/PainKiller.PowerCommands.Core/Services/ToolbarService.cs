using System.Drawing;
using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration.Enums;
using PainKiller.PowerCommands.ReadLine;

namespace PainKiller.PowerCommands.Core.Services;
public static class ToolbarService
{
    private static string[]? _labels;
    private static readonly Dictionary<string, string[]> ContextualToolbar = new();
    public static TEnum NavigateToolbar<TEnum>(ConsoleColor[]? consoleColors = null, int padLeft = 1, string title = "Use tab to select, then hit [RETURN]") where TEnum : Enum
    {
        ConsoleService.Service.WriteHeaderLine(nameof(NavigateToolbar), title.PadLeft(title.Length + padLeft));
        var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        var labels = enumValues
            .OrderBy(e => Convert.ToInt32(e))
            .Select(e => e.ToString())
            .ToArray();
        var selectedLabel = NavigateToolbar(labels, consoleColors, padLeft: padLeft);
        ConsoleService.Service.ClearRow(Console.CursorTop);
        ConsoleService.Service.WriteSuccessLine(nameof(NavigateToolbar), selectedLabel.PadLeft(selectedLabel.Length + padLeft));
        return (TEnum)Enum.Parse(typeof(TEnum), selectedLabel);
    }
    public static string NavigateToolbar(string[] labels, ConsoleColor[]? consoleColors = null, int padLeft = 1)
    {
        try
        {
            if (labels.Length == 0) return "";
            labels = labels.Reverse().ToArray();

            var colors = consoleColors ??
            [
                ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.Red, ConsoleColor.DarkBlue, ConsoleColor.Blue,
                ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.DarkYellow, ConsoleColor.DarkBlue
            ];
            colors = colors.Reverse().ToArray();

            var currentIndex = labels.Length - 1;
            ConsoleKey key;
            do
            {
                DrawToolbar(labels, colors, showOnBottom: false, currentIndex, padLeft: padLeft);
                Console.Title = labels[currentIndex];
                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Tab)
                {
                    currentIndex = (currentIndex - 1) % labels.Length;
                    if (currentIndex < 0) currentIndex = labels.Length - 1;
                }
                else if (key == ConsoleKey.Enter)
                {
                    return labels[currentIndex];
                }

            } while (key != ConsoleKey.Escape);

            return "";
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            return "";
        }
    }
    public static void AddContextualToolbar(string id, string[] labels) => ContextualToolbar.TryAdd(id, labels);
    public static void DrawContextualToolbar(string id, string[]? defaultLabels = null, ConsoleColor[]? consoleColors = null, bool showOnBottom = true, int selectedIndex = -1, int padLeft = 1)
    {
        if (ContextualToolbar.Count == 0) return;
        var labels = ContextualToolbar.ContainsKey(id) ? ContextualToolbar.First(i => i.Key == id).Value : defaultLabels ?? [];
        if (labels.Length == 0) labels = defaultLabels;
        DrawToolbar(labels!, consoleColors, showOnBottom, selectedIndex, padLeft);
    }
    public static void DrawToolbar<TEnum>(ConsoleColor[]? consoleColors = null, bool showOnBottom = true, int padLeft = 1) where TEnum : Enum
    {
        var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        var labels = enumValues
            .OrderBy(e => Convert.ToInt32(e))
            .Select(e => e.ToString())
            .ToArray();
        DrawToolbar(labels, consoleColors, showOnBottom, padLeft: padLeft);
    }
    public static void DrawToolbar(string[] labels, ConsoleColor[]? consoleColors = null, bool showOnBottom = true, int selectedIndex = -1, int padLeft = 1)
    {
        try
        {
            if (labels.Length == 0) return;
            ClearToolbar();

            var originalPosition = new Point(Console.CursorLeft, Console.CursorTop);
            var toolbarY = showOnBottom ? Console.WindowHeight - 2 : originalPosition.Y;

            Console.SetCursorPosition(0, toolbarY);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(padLeft, toolbarY);

            _labels = labels;
            var colors = consoleColors ??
            [
                ConsoleColor.Green, ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow,
                ConsoleColor.DarkGray, ConsoleColor.Red, ConsoleColor.DarkBlue, ConsoleColor.DarkGreen, ConsoleColor.Blue,
                ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.DarkYellow, ConsoleColor.DarkBlue
            ];

            colors = colors.Reverse().ToArray();
            var colorIndex = 0;
            var width = 0;

            //Console.Write(" ".PadLeft(padLeft));

            foreach (var label in labels.Reverse())
            {
                var displayLabel = label;
                var color = colors[colorIndex++ % colors.Length];
                if (Array.IndexOf(labels, label) == selectedIndex)
                {
                    displayLabel = $"[{label}]";
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = color;
                }

                width += displayLabel.Length + 2;
                if (showOnBottom) Console.SetCursorPosition(Math.Clamp(Console.WindowWidth - width, 0, Console.WindowWidth), Math.Clamp(Console.WindowHeight - 2, 0, Console.WindowHeight));
                Console.Write($" {displayLabel} ");
                Console.ResetColor();
                Console.Write(" ");
            }
            Console.ResetColor();
            Console.SetCursorPosition(originalPosition.X, originalPosition.Y);
        }
        catch (Exception e)
        {
            ClearToolbar();
            IPowerCommandServices.DefaultInstance?.Logger.LogError(e.Message);
        }
    }

    public static void ClearToolbar()
    {
        try
        {
            if (_labels == null || _labels.Length == 0) return;
            var originalPosition = new Point(x: Console.CursorLeft, y: Console.CursorTop);
            var width = _labels.Sum(label => label.Length + 1);
            Console.SetCursorPosition(Math.Clamp(Console.WindowWidth - (width + _labels.Length), 0, Console.WindowWidth), Math.Clamp(Console.WindowHeight - 2, 0, Console.WindowHeight));
            Console.Write("".PadLeft(width + _labels.Length, ' '));
            Console.SetCursorPosition(originalPosition.X, originalPosition.Y);
        }
        catch (Exception e)
        {
            IPowerCommandServices.DefaultInstance?.Logger.LogError(e.Message);
        }
    }

    private static ToolbarConfiguration? _configuration;
    public static void DrawToolbar(ToolbarConfiguration? configuration)
    {
        _configuration = configuration;
        if (_configuration == null) return;
        DrawToolbar(_configuration.ToolbarItems.Select(t => t.Label).ToArray(), _configuration.ToolbarItems.Select(t => t.Color).ToArray());
        if (_configuration.HideToolbarOption == HideToolbarOption.OnTextChange) ReadLineService.CmdLineTextChanged += ReadLineService_CmdLineTextChanged;
        else if (_configuration.HideToolbarOption == HideToolbarOption.OnCommandHighlighted) ReadLineService.CommandHighlighted += ReadLineService_CommandHighlighted;
    }

    private static void ReadLineService_CommandHighlighted(object? sender, ReadLine.Events.CommandHighlightedArgs e)
    {
        if (_configuration?.ToolbarItems == null) return;
        ClearToolbar();
        ReadLineService.CommandHighlighted -= ReadLineService_CommandHighlighted;
    }

    private static void ReadLineService_CmdLineTextChanged(object? sender, ReadLine.Events.CmdLineTextChangedArgs e)
    {
        if (_configuration?.ToolbarItems == null) return;
        ClearToolbar();
        ReadLineService.CmdLineTextChanged -= ReadLineService_CmdLineTextChanged;
    }
}