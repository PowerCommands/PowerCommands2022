using GlitchFinder.DataSources;
using GlitchFinder.Managers;
using GlitchFinder.Reporters;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

public class RegressionProject
{
    public string Name { get; set; } = "Regression project 1";
    public RegressionTestSetting Settings { get; set; } = new()
    {
        ComparisonFields = new []{"Column1","Column2"},
        SourceSetting = new() { ConnectionString = "Server=serverName;Database=DbName;User Id=databaseUser;Password=#PASSWORD#;", DataSourceType = DataSourceType.MsSql, Query = "SELECT * FROM TABLE1", UniqueKeyFields = new[] { "Id" } },
        BaselineFilePath = "baseline-data.json",
        ReportFilePath = "Reports",
        ReportType = ReportType.Html
    };
}