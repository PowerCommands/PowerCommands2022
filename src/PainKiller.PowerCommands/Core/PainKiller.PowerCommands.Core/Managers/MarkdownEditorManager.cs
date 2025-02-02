using PainKiller.PowerCommands.Shared.Extensions;

namespace PainKiller.PowerCommands.Core.Managers;

public class MarkdownEditorManager
{
    private const string Exit = "/XT";
    private readonly MarkdownFileService _markdownService = MarkdownFileService.Instance;
    private readonly string _filePath;
    private readonly IConsoleService _consoleService;
    private readonly string _prompt;

    private readonly List<string> _commands =
    [
        Exit,
        "#", "##", "###",
        "---", // Horizontal rule
        "`code`", // Code
        "*italic*", "**bold**", "***bold italic***"
    ];

    public MarkdownEditorManager(string filePath, IConsoleService consoleService, string prompt)
    {
        _filePath = filePath;
        _consoleService = consoleService;
        _prompt = prompt;
        Console.WriteLine($"Markdown Editor - Start writing your Markdown line by line. Use '{Exit}' to save the markdown file.");
        Console.WriteLine("Use tabs to get suggestion of what you can add to the document, just the simplest stuff supported.");
        Console.WriteLine("The document will be opened in your editor of choice when you are done here.");
    }
    public void Run()
    {
        while (true)
        {
            Console.Write($"{_prompt} ");
            var input = _consoleService.ReadInputWithCompletion(_commands);
            
            if (input.Equals(Exit, StringComparison.OrdinalIgnoreCase))
            {
                SaveToFile();
                Console.WriteLine($"Markdown file saved to {_filePath}");
                break;
            }
            
            ProcessInput(input);
        }
    }
    private void ProcessInput(string input)
    {
        if (input.StartsWith("#"))
        {
            int level = input.TakeWhile(c => c == '#').Count();
            string text = input.TrimStart('#').Trim();
            _markdownService.AddHeader(text, level);
        }
        else if (input.StartsWith("---"))
        {
            _markdownService.AddHorizontalRule();
        }
        else if (input.StartsWith("`code`"))
        {
            _markdownService.AddCode(string.Join("\n", input.Replace("`code`", "")));
        }
        else if (input.StartsWith("*italic*"))
        {
            _markdownService.AddParagraph($"*{input.Replace("*italic*","")}*");
        }
        else if (input.StartsWith("**bold**"))
        {
            _markdownService.AddParagraph($"**{input.Replace("**bold**","")}**");
        }
        else if (input.StartsWith("***bold italic***"))
        {
            _markdownService.AddParagraph($"***{input.Replace("***bold italic***","")}***");
        }
        else
        {
            _markdownService.AddParagraph(input);
        }
    }
    private void SaveToFile() => File.WriteAllText(_filePath, _markdownService.ToString());
}