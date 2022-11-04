using GlitchFinder.DataSources;
using GlitchFinder.Managers;
using GlitchFinder.Matrix.Contracts;
using GlitchFinder.Reporters;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.GlitchFinderCommands.BaseClasses;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;
namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[PowerCommandDesign(description: "Creates a new project in config file from a template.\nA folder with the given project name in the projects directory will be created.",
                      arguments: "!comparison|regression",
                         quotes: "!project name:<name>",
                        example: "create comparison \"My comparison project\"|create regression \"My regression test project\"")]
public class CreateCommand : GlitchFinderBaseCommand
{
    public CreateCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if(string.IsNullOrEmpty(Input.SingleArgument)) return BadParameterError("You must provide witch type of project to create, comparison or regression");
        if (string.IsNullOrEmpty(Input.SingleQuote)) return BadParameterError("The project needs a name");

        var projectName = Input.SingleQuote;
        ProjectPath = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName);

        CreateProjectDirectory(projectName);

        if(Input.SingleArgument == "comparison") NewComparison(projectName);
        if(Input.SingleArgument == "regression") NewRegressionTest(projectName);
        
        return Ok();
    }
    public void NewComparison(string projectName)
    {
        Configuration.ComparisonProjects.Add(new(){Name = projectName});
        ConfigurationService.Service.SaveChanges(Configuration);
        
        var settings = new ComparisonSetting
        {
            ComparisonFields = new List<ComparisonField>() { new() { LeftFieldName = "Column1", RightFieldName = "Column2" } },
            LeftDataSource = new() { ConnectionString = "Server=serverName;Database=DbName;User Id=databaseUser;Password=#PASSWORD#;", DataSourceType = DataSourceType.MsSql, Query = "SELECT * FROM TABLE1", UniqueKeyFields = new[] { "Id" } },
            RightDataSource = new() { DataSourceType = DataSourceType.CsvFile, FilePath = "data.csv", Separator = ";", UniqueKeyFields = new[] { "Id" } },
            ReportFilePath = "Reports.html",
            ReportType = ReportType.Html
        };
        var fileName = Path.Combine(ProjectPath, $"{ComparisonConfigFileName}");
        if(!File.Exists(fileName)) ConfigurationService.Service.Create(settings, fileName);

        WriteLine($"A new comparison project \"{projectName}\" has been created");
    }
    public void NewRegressionTest(string projectName)
    {
        Configuration.RegressionProjects.Add(new(){Name = projectName});
        ConfigurationService.Service.SaveChanges(Configuration);
        var settings = new RegressionTestSetting
        {
            ComparisonFields = new[] { "Column1", "Column2" },
            SourceSetting = new() { ConnectionString = "Server=serverName;Database=DbName;User Id=databaseUser;Password=#PASSWORD#;", DataSourceType = DataSourceType.MsSql, Query = "SELECT * FROM TABLE1", UniqueKeyFields = new[] { "Id" } },
            BaselineFilePath = "baseline-data.json",
            ReportFilePath = "Reports.html",
            ReportType = ReportType.Html
        };
        var fileName = Path.Combine(ProjectPath, $"{RegressionTestConfigFileName}");
        if (!File.Exists(fileName)) ConfigurationService.Service.Create(settings, fileName);

        WriteLine($"A new configuration template \"{projectName}\" has been created");
    }
    private void CreateProjectDirectory(string projectName)
    {
        var projectsRootPath = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath);
        if (!Directory.Exists(projectsRootPath)) Directory.CreateDirectory(projectsRootPath);

        var projectsPath = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName);
        if (!Directory.Exists(projectsPath)) Directory.CreateDirectory(projectsPath);
    }
}