namespace PainKiller.PowerCommands.DrontlogCommands.Entities;

public class TrendItem
{
    public string Title { get; set; } = "";
    public string Summary { get; set; } = "";
    public string UrlToImage { get; set; } = "";
    public List<NewsItem> News { get; set; } = new();
}