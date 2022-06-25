using System.Data.SqlClient;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Dapper;

namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

[Tags("rss|feed|example")]
[PowerCommand(description: "Download and parse a RSS/Atom feed",
    example: "Syndication https://trends.google.se/trends/trendingsearches/daily/rss?geo=SE")]
public class SyndicationCommand : DapperCommandBase
{
    public SyndicationCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
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
        SavePost(trendItems);
    }

    private void SavePost(List<TrendItem> trendItems)
    {
        var serviceName = "Google Trends SE";
        using var connection = new SqlConnection(ConnectionString);
        var providerService = connection.QueryFirst<ProviderService>($"SELECT {nameof(ProviderService.ProviderServiceID)} FROM {Schema}.{nameof(ProviderServiceReplica)} WHERE [{nameof(ProviderService.Name)}] = @ServiceName", new {ServiceName = serviceName});

        var post = new Post {PostID = Guid.NewGuid(), Caption = serviceName, UrlToLogo = providerService.UrlToLogo, Created = DateTime.Now, ProviderServiceID = providerService.ProviderServiceID, ProviderSpecificID = Guid.NewGuid().ToString(), ProviderSpecificCreated = DateTime.Now, OwnerProviderServiceID = providerService.ProviderServiceID};
        var postContent = new PostContent {PostID = post.PostID};
        var content = new StringBuilder();
        foreach (var trend in trendItems)
        {
            content.AppendLine($"<h2>{trend.Title}</h2>");
            content.AppendLine("<p>");
            content.AppendLine($"<img src=\"{trend.UrlToImage}\" /><br/>");
            content.AppendLine($"{trend.Summary}");
            content.AppendLine("</p>");
            
            foreach (var newsItem in trend.News)
            {
                content.AppendLine("<p>");
                content.AppendLine($"<b>{newsItem.Title}</b><br/>");
                content.AppendLine($"<i>{newsItem.Text}</i><br/>");
                content.AppendLine($"<a href=\"{newsItem.Url}\">{newsItem.Source}</a><br/>");
                content.AppendLine("</p>");
            }
        }
        postContent.MainBody = content.ToString();

        connection.Query("INSERT INTO [timeline].[Post] ([PostID],[ProviderServiceID],[OwnerProviderServiceID],[ProviderSpecificID],[ProviderSpecificCreated],[Caption],[PublishStatusID],[UrlToLogo],[CommentCount]) VALUES( @PostID,@ProviderServiceID,@OwnerProviderServiceID,@ProviderSpecificID,@ProviderSpecificCreated,@Caption,@PublishStatusID,@UrlToLogo,@CommentCount)", post);
        connection.Query("INSERT INTO [timeline].[PostContent] ([PostID],[MainBody],[Tags]) VALUES( @PostID,@MainBody,@Tags)", postContent);
    }
}