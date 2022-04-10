using PainKiller.PowerCommands.ReadLine.Contracts;
using PainKiller.PowerCommands.ReadLine.DomainObjects;
using PainKiller.PowerCommands.ReadLine.Handlers;
using PainKiller.PowerCommands.ReadLine.Managers;
using static System.String;

namespace PainKiller.PowerCommands.ReadLine;
public class ReadLineService
{
    private readonly List<string> _history = new();
    private static readonly Lazy<ReadLineService> Lazy = new(() => new ReadLineService());
    public static ReadLineService Service => Lazy.Value;

    public static void InitializeAutoComplete(string[] history, string[] suggestions)
    {
        if (history.Length > 0) Service.AddHistory(history);
        Service.AutoCompletionHandler = new AutoCompleteHandler(suggestions, new SuggestionProviderManager().SuggestionProviderFunc);
    }
    public void AddHistory(params string[] text) => _history.AddRange(text);
    public IAutoCompleteHandler AutoCompletionHandler { private get; set; } = null!;
    public string Read(string prompt = "", string @default = "")
    {
        Console.Write(prompt);
        var keyHandler = new KeyHandler(new Console2(), _history, AutoCompletionHandler);
        var text = GetText(keyHandler);

        if (IsNullOrWhiteSpace(text) && !IsNullOrWhiteSpace(@default)) text = @default;
        _history.Add(text);
        return text;
    }

    //Keeping this in case I want to use it later on...
    //public string ReadPassword(string prompt = "")
    //{
    //    Console.Write(prompt);
    //    KeyHandler keyHandler = new KeyHandler(new Console2() { PasswordMode = true }, new List<string>(), null!);
    //    return GetText(keyHandler);
    //}
    private string GetText(KeyHandler keyHandler)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        while (keyInfo.Key != ConsoleKey.Enter)
        {
            keyHandler.Handle(keyInfo);
            keyInfo = Console.ReadKey(true);
        }

        Console.WriteLine();
        return keyHandler.Text;
    }
}