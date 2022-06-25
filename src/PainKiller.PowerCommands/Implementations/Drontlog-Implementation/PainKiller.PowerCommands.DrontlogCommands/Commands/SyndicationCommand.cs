using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;

namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

[Tags("rss|feed|example")]
[PowerCommand(description: "Download and parse a RSS/Atom feed",
    example: "Syndication https://trends.google.se/trends/trendingsearches/daily/rss?geo=SE")]
public class SyndicationCommand : CommandBase<CommandsConfiguration>
{
    public SyndicationCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var url = "https://trends.google.se/trends/trendingsearches/daily/rss?geo=SE";
        TryRss20FeedFormat(url);
        return CreateRunResult(input);
    }

    private void TryRss20FeedFormat(string url)
    {
        using var reader = XmlReader.Create(url);
        var formatter = new Rss20FeedFormatter();
        formatter.ReadFrom(reader);
        var feed = formatter.Feed;
        var feedRss20 = feed.Items;

        var trendItems = new List<TrendItem>();

        foreach (var item in feedRss20)
        {
            var trendItem = new TrendItem{Title = item.Title.Text,Summary = item.Summary.Text};
            foreach (var extension in item.ElementExtensions)
            {
                var xItemElement = extension.GetObject<XElement>();
                if (xItemElement.Name.LocalName == "news_item")
                    trendItem.News.Add(new NewsItem(xItemElement.Value));
                if (xItemElement.Name.LocalName == "picture")
                    trendItem.UrlToImage = xItemElement.Value;
            }
            trendItems.Add(trendItem);
        }

        foreach (var trend in trendItems)
        {
            Console.WriteLine(trend.Title);
            Console.WriteLine(trend.Summary);
            Console.WriteLine(trend.UrlToImage);
            Console.WriteLine("News");
            foreach (var newsItem in trend.News)
            {
                Console.WriteLine($"News: {newsItem.Title} {newsItem.Text} {newsItem.Url} {newsItem.Source}");
            }
        }
    }

    private void TryAtom10FeedFormat(string url)
    {
        using var reader = XmlReader.Create(url);
        var formatter = new Atom10FeedFormatter();
        formatter.ReadFrom(reader);

        var feedAtom10 = formatter.Feed.Items;
        foreach (var item in feedAtom10)
        {
            if (item.Content == null) continue;
            Console.WriteLine(item.Content.AttributeExtensions.TryGetValue(new XmlQualifiedName("text"), out var mainBody) ? mainBody : "no content");
        }
    }
}