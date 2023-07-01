using PainKiller.PowerCommands.ReadLine;
using PainKiller.PowerCommands.Security.Services;

namespace $safeprojectname$.Services;
public static class DialogService
{
    public static bool YesNoDialog(string question, string yesValue = "y", string noValue = "n")
    {
        Console.Write($"\n{question} ({yesValue}/{noValue}): ");
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
    public static string SecretPromptDialog(string question, int maxRetries = 3)
    {
        var retryCount = 0;
        var secret = "";
        while (retryCount < maxRetries)
        {
            Console.Write($"\n{question} :");
            secret = PasswordPromptService.Service.ReadPassword();
            Console.WriteLine();
            Console.Write("Confirm: ".PadLeft(question.Length));
            var confirm = PasswordPromptService.Service.ReadPassword();
            if (secret != confirm)
            {
                ConsoleService.Service.WriteCritical(nameof(DialogService),"\nConfirmation failure, please try again.\n");
                retryCount++;
            }
            else break;
        }
        
        return $"{secret}".Trim();
    }
    public static List<string> ListDialog(string header, List<string> items, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Blue)
    {
        Console.Clear();
        ConsoleService.Service.WriteHeaderLine(nameof(DialogService), header);
        var startRow = Console.CursorTop;
        var startForegroundColor = Console.ForegroundColor;
        var startBackgroundColor = Console.BackgroundColor;

        for (int index=0; index<items.Count;index++)
        {
            var item = items[index];
            Console.WriteLine($" {index+1}. {item}");
        }
        var quit = " ";
        var input = "";
        var selectedItems = new Dictionary<int, string>();
        
        ToolbarService.DrawToolbar(new []{"Select number, hit enter","When your done just hit enter."});
        while (input != quit)
        {
            Console.WriteLine("");
            Console.Write(" ");
            input = ReadLineService.Service.Read();
            if(input.Trim() == "") break;
            
            var selectedIndex = (int.TryParse(input, out var index) ? index : 1);
            if(selectedIndex > items.Count-1) selectedIndex = items.Count;
            var selectedItem = new { Index = selectedIndex, Value = items[selectedIndex - 1] };
            var itemAdded = selectedItems.TryAdd(selectedItem.Index, selectedItem.Value);
            
            ConsoleService.Service.ClearRow(Console.CursorTop-1);
            var top = Console.CursorTop - 2;
            Console.CursorTop = Math.Clamp(top, 0, Console.LargestWindowHeight - 1);
            Console.CursorLeft = 0;

            if(itemAdded) ConsoleService.Service.WriteRowWithColor(startRow + selectedIndex - 1, foregroundColor, backgroundColor, $" {selectedIndex}. {selectedItem.Value}");
            else
            {
                selectedItems.Remove(selectedItem.Index);
                ConsoleService.Service.WriteRowWithColor(startRow + selectedIndex - 1, startForegroundColor, startBackgroundColor, $" {selectedIndex}. {selectedItem.Value}");
            }
        }
        ToolbarService.ClearToolbar();
        return selectedItems.Select(s => s.Value).ToList();
    }
}