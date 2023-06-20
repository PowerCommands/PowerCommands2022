using System.Drawing;

namespace PainKiller.PowerCommands.Core.Services;
public static class DialogService
{
    public static bool YesNoDialog(string question, string yesValue = "y", string noValue = "n")
    {
        Console.WriteLine($"\n{question} ({yesValue}/{noValue}):");
        var response = Console.ReadLine();
        return $"{response}".Trim().ToLower() == yesValue.ToLower();
    }
    public static string QuestionAnswerDialog(string question)
    {
        Console.WriteLine($"\n{question}");
        Console.Write(ConfigurationGlobals.Prompt);
        var response = Console.ReadLine();
        return $"{response}".Trim();
    }
    public static void DrawToolbar(string[] labels, ConsoleColor[]? consoleColors = null)
    {
        var colors = consoleColors ?? new[] { ConsoleColor.Green, ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.DarkGray,ConsoleColor.Red,ConsoleColor.DarkBlue,ConsoleColor.DarkGreen };
        colors = colors.Reverse().ToArray();
        var originalPosition = new Point(x: Console.CursorLeft, y: Console.CursorTop);
        var colorIndex = 0;
        var width = 0;
        var startBackgroundColor = Console.BackgroundColor;

        foreach (var label in labels.Reverse().Take(colors.Length))
        {
            var color = colors[colorIndex++];
            width += label.Length + 1;
            Console.SetCursorPosition(Math.Clamp(Console.WindowWidth - width, 0, Console.WindowWidth), Math.Clamp(Console.WindowHeight - 2, 0, Console.WindowHeight));
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = color;
            Console.Write(label);
            Console.BackgroundColor = startBackgroundColor;
            Console.Write(" ");
        }
        Console.ResetColor();
        Console.SetCursorPosition(originalPosition.X, originalPosition.Y);
    }
    public static void ClearToolbar(string[] labels)
    {
        if(labels.Length == 0) return;
        var originalPosition = new Point(x: Console.CursorLeft, y: Console.CursorTop);
        var width = 0;
        foreach (var label in labels) width += label.Length+1; 
        Console.SetCursorPosition( Math.Clamp(Console.WindowWidth - width, 0, Console.WindowWidth),  Math.Clamp(Console.WindowHeight-2, 0, Console.WindowHeight));
        Console.Write("".PadLeft(width,' '));
        Console.SetCursorPosition(originalPosition.X, originalPosition.Y);
    }
}