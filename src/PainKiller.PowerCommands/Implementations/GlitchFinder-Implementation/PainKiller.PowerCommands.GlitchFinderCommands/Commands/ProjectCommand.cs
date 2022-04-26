using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[Tags("project|help")]
[PowerCommand( description: "Project management",
                 arguments: "action:log",
                     qutes: "project:<name>",
                   example: "projects")]
public class ProjectsCommand : CommandBase<PowerCommandsConfiguration>
{
    public ProjectsCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (input.SingleArgument == "log" && !string.IsNullOrEmpty(input.SingleQuote))
        {
            ViewLog(input.SingleQuote);
            return CreateRunResult(input);
        }

        WriteHeadLine("Comparison projects");
        foreach (var c in Configuration.ComparisonProjects) this.WriteObjectDescription(c.Name, $"Left: {c.Settings.LeftDataSource.DataSourceType} Right: {c.Settings.RightDataSource.DataSourceType} Comparsion fields: {string.Join(',', c.Settings.ComparisonFields.Select(s => s.RightFieldName))}");
        WriteHeadLine("Regression test projects");
        foreach (var r in Configuration.RegressionProjects) this.WriteObjectDescription(r.Name, $"Source: {r.Settings.SourceSetting.DataSourceType} BaselineFilePath: {r.Settings.BaselineFilePath} Comparsion fields: {string.Join(',', r.Settings.ComparisonFields)}");
        return CreateRunResult(input);
    }
    private void ViewLog(string projectName)
    {
        foreach (var line in Configuration.Log.GetProcessLog(projectName)) Console.WriteLine(line);
    }
}