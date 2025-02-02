namespace PainKiller.PowerCommands.Shared.Contracts;
public interface IMarkdownFileService
{
    void AddHeader(string text, int level = 1);
    void AddParagraph(string text);
    void AddBulletList(IEnumerable<string> items);
    void AddNumberedList(IEnumerable<string> items);
    void AddCodeBlock(string code);
    void AddCode(string code);
    void AddHorizontalRule();
    void AddLink(string text, string url);
    void AddImage(string altText, string imageUrl);
    string ToString();
}