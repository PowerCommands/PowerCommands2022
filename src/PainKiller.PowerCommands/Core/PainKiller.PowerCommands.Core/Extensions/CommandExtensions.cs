using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Extensions;

public static class CommandExtensions
{
    public static void WriteObjectDescription(this IConsoleCommand command, string name, string description)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"{name}: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{description}");
        Console.ForegroundColor = currentColor;
    }
    public static void Write(this IConsoleCommand command, string text, ConsoleColor color)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = currentColor;
    }

    public static void WriteLine(this IConsoleCommand command, string text, ConsoleColor color)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = currentColor;
    }
    public static void WriteHeaderLine(this IConsoleCommand command, string text, ConsoleColor color = ConsoleColor.DarkGreen)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = currentColor;
    }
}