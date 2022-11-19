namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

[PowerCommandTest(tests:"--days 7|--weeks 4|--days 21 --url")]
[PowerCommandDesign(description: "List the latest added knowledge documents.",
    options: "!days|!weeks|url|path|onenote|file",
    example: "//Show created items the last 3 days|latest --days 3|//Show created items the last 4 weeks.|latest --weeks 4|//Show all created files the last week|latest --week 1 --file")]
public class LatestCommand : DisplayCommandsBase
{
    public LatestCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration){}
    public override RunResult Run()
    {
        var sourceType = Input.GetOptionValue(new[] { "url", "path", "url", "file" });
        var dayFactor = Input.HasOption("weeks") ? 7 : 1;
        var number = 0;
        if (int.TryParse(Input.GetOptionValue("weeks"), out var index)) number = index;
        if (int.TryParse(Input.GetOptionValue("days"), out var index2)) number = index2;

        var latestDate = number == 0 ? DateTime.Now.AddDays(-10000) : DateTime.Now.AddDays(-(dayFactor*number));
        Items = Storage.GetObject().Items.Where(i => i.Created > latestDate && (i.SourceType == sourceType || string.IsNullOrEmpty(sourceType))).ToList();
        Print();
        return Ok();
    }
}