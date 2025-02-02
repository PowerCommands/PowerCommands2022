using System.Text;
namespace PainKiller.PowerCommands.Core.Services;

public class MarkdownFileService : IMarkdownFileService
{
    private static MarkdownFileService? _instance;
    private MarkdownFileService() { }
    public static MarkdownFileService Instance => _instance ??= new MarkdownFileService();

    private readonly StringBuilder _content = new();
    public void AddHeader(string text, int level = 1) => _content.AppendLine($"{new string('#', Math.Clamp(level, 1, 3))} {text}\n");
    public void AddParagraph(string text) => _content.AppendLine($"{text}\n");
    public void AddBulletList(IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            _content.AppendLine($"- {item}");
        }
        _content.AppendLine();
    }
    public void AddNumberedList(IEnumerable<string> items)
    {
        int index = 1;
        foreach (var item in items)
        {
            _content.AppendLine($"{index}. {item}");
            index++;
        }
        _content.AppendLine();
    }
    public void AddCodeBlock(string code) => _content.AppendLine($"```\n{code}\n```\n");
    public void AddCode(string code) => _content.AppendLine($"`{code}`");
    public void AddHorizontalRule() => _content.AppendLine("---\n");
    public void AddLink(string text, string url) => _content.AppendLine($"[{text}]({url})\n");
    public void AddImage(string altText, string imageUrl) => _content.AppendLine($"![{altText}]({imageUrl})\n");
    public override string ToString() => _content.ToString();
}