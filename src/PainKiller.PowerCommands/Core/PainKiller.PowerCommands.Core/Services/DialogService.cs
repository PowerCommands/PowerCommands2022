using PainKiller.PowerCommands.ReadLine;
using PainKiller.PowerCommands.Security.Services;

namespace PainKiller.PowerCommands.Core.Services;
public static class DialogService
{
    public static bool YesNoDialog(string question, string yesValue = "y", string noValue = "n")
    {
        WriteHeader($"\n{question}"); ;
        Console.WriteLine($"({yesValue}/{noValue}):");
        
        var response = Console.ReadLine();
        return $"{response}".Trim().ToLower() == yesValue.ToLower();
    }
    public static string QuestionAnswerDialog(string question)
    {
        WriteHeader($"{question}\n");
        Console.Write(ConfigurationGlobals.Prompt);
        var response = Console.ReadLine();
        return $"{response}".Trim();
    }
    public static string SecretPromptDialog(string question, int maxRetries = 3)
    {
        var retryCount = 0;
        var secret = "";
        while (retryCount < maxRetries)
        {
            WriteHeader($"\n{question} ");
            secret = PasswordPromptService.Service.ReadPassword();
            Console.WriteLine();
            Console.Write("Confirm: ".PadLeft(question.Length + 1));
            var confirm = PasswordPromptService.Service.ReadPassword();
            if (secret != confirm)
            {
                ConsoleService.Service.WriteCritical(nameof(DialogService), "\nConfirmation failure, please try again.\n");
                retryCount++;
            }
            else break;
        }

        return $"{secret}".Trim();
    }
    public static Dictionary<int, string> ListDialog(string header, List<string> items, bool multiSelect = false, bool autoSelectIfOnlyOneItem = true, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Blue)
    {
        if (items.Count == 0) return new Dictionary<int, string>();
        Console.Clear();
        WriteHeader($"{header}\n");
        var startRow = Console.CursorTop;
        var startForegroundColor = Console.ForegroundColor;
        var startBackgroundColor = Console.BackgroundColor;

        for (int index=0; index<items.Count;index++)
        {
            var item = items[index];
            Console.WriteLine($"{index+1}. {item}");
        }
        var quit = " ";
        var input = "";
        var selectedItems = new Dictionary<int, string>();

        if (items.Count == 1 && autoSelectIfOnlyOneItem)
        {
            selectedItems.Add(0, items.First());
            Console.WriteLine("");
            Console.Write(" ");
            ConsoleService.Service.WriteRowWithColor(1, foregroundColor, backgroundColor, $" 1. {items.First()}");
            return selectedItems;
        }

        var label = multiSelect ? "Enter number(s) and hit enter" : "Enter a number and hit enter";
        while (input != quit)
        {
            Console.WriteLine("");
            Console.Write($"{label}>");
            input = ReadLineService.Service.Read();
            if(input.Trim() == "") break;
            
            var selectedIndex = (int.TryParse(input, out var index) ? index : 1);
            if(selectedIndex > items.Count || selectedIndex < 1) selectedIndex = 1;
            var selectedItem = new { Index = selectedIndex, Value = items[selectedIndex - 1] };
            var itemAdded = selectedItems.TryAdd(selectedItem.Index-1, selectedItem.Value);
            
            ConsoleService.Service.ClearRow(Console.CursorTop-1);
            var top = Console.CursorTop - 2;
            Console.CursorTop = Math.Clamp(top, 0, Console.LargestWindowHeight - 1);
            Console.CursorLeft = 0;

            if(itemAdded) ConsoleService.Service.WriteRowWithColor(startRow + selectedIndex - 1, foregroundColor, backgroundColor, $"{selectedIndex}. {selectedItem.Value}");
            else
            {
                selectedItems.Remove(selectedItem.Index);
                ConsoleService.Service.WriteRowWithColor(startRow + selectedIndex - 1, startForegroundColor, startBackgroundColor, $"{selectedIndex}. {selectedItem.Value}");
            }

            if (!multiSelect) break;
        }
        ToolbarService.ClearToolbar();
        return selectedItems;
    }
    private static void WriteHeader(string text)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(text);
        Console.ForegroundColor = originalColor;
    }
}