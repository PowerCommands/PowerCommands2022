using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.Extensions;
public static class ConsoleExtensions
{
    public static string ReadInputWithCompletion(this IConsoleService consoleService, IEnumerable<string> suggestions)
    {
        var input = "";
        var matchingSuggestions = new List<string>();
        var suggestionIndex = -1;

        var suggestionList = suggestions.ToList();

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return input;
            }
            if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input[..^1];
                Console.Write("\b \b");
            }
            else if (key.Key == ConsoleKey.Tab)
            {
                if (!matchingSuggestions.Any())
                    matchingSuggestions = suggestionList.Where(c => c.StartsWith(input, StringComparison.OrdinalIgnoreCase)).ToList();

                if (!matchingSuggestions.Any()) continue;
                suggestionIndex = (suggestionIndex + 1) % matchingSuggestions.Count;
                input = matchingSuggestions[suggestionIndex];
                Console.Write($"\r:> {input}   ");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                input += key.KeyChar;
                Console.Write(key.KeyChar);
                matchingSuggestions.Clear();
                suggestionIndex = -1;
            }
        }
    }
}