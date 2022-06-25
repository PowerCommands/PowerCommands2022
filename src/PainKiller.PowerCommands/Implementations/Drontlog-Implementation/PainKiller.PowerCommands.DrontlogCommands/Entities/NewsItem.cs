namespace PainKiller.PowerCommands.DrontlogCommands.Entities;
public class NewsItem
{
    public NewsItem(string xElementAsString)
    {
        var rows = xElementAsString.Split('\n');
        Title = rows.Length > 0 ? $"{rows[0].Trim()}" : "";
        Text = rows.Length > 1 ? $"{rows[1].Trim()}" : "";
        Url = rows.Length > 2 ? $"{rows[2].Trim()}" : "";
        Source = rows.Length > 3 ? $"{rows[3].Trim()}" : "";
    }
    public string Title { get; set; }
    public string Text { get; set; }
    public string Url { get; set; }
    public string Source { get; set; }

}