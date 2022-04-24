using GlitchFinder.DataSources;
using GlitchFinder.Managers;
using GlitchFinder.Matrix.Contracts;
using GlitchFinder.Reporters;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

public class ComparisonProject
{
    public string Name { get; set; } = "Comparison project 1";
    public ComparisonSetting Settings { get; set; } = new()
    {
        ComparisonFields = new List<ComparisonField>() {new() {LeftFieldName = "Column1", RightFieldName = "Column2"}},
        LeftDataSource = new() {ConnectionString = "Server=serverName;Database=DbName;User Id=databaseUser;Password=#PASSWORD#;", DataSourceType = DataSourceType.MsSql, Query = "SELECT * FROM TABLE1", UniqueKeyFields = new[] { "Id" } },
        RightDataSource = new() {DataSourceType = DataSourceType.CsvFile,FilePath = "data.csv",Separator = ";",UniqueKeyFields = new []{"Id"}},
        ReportFilePath = "Reports",
        ReportType = ReportType.Html
    };
}