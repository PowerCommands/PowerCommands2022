namespace PainKiller.PowerCommands.DrontlogCommands.Entities;


public class QuoteOfTheDay
{
    public DateTime qotd_date { get; set; }
    public Quote quote { get; set; }
}

public class Quote
{
    public int id { get; set; }
    public bool dialogue { get; set; }
    public bool _private { get; set; }
    public string[] tags { get; set; }
    public string url { get; set; }
    public int favorites_count { get; set; }
    public int upvotes_count { get; set; }
    public int downvotes_count { get; set; }
    public string author { get; set; }
    public string author_permalink { get; set; }
    public string body { get; set; }
}
