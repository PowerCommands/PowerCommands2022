using GlitchFinder.DataSources;
using GlitchFinder.Matrix;
using GlitchFinder.Matrix.Contracts;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.GlitchFinderCommands.BaseClasses;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[Tags("comparsion")]
[PowerCommand(description: "Compare to datasources with each other, find glitches, if any...",
                arguments: "project:<name>",
        argumentMandatory: true,
                  example: "comparison \"My sample project\"")]
public class ComparisonCommand : GlitchFinderBaseCommand
{
    public ComparisonCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleQuote)) return CreateBadParameterRunResult(input, "You must provide a project name, project must exist...");
        var isEqual = Compare(input.SingleQuote);
        if(isEqual) WriteHeadLine("No glitches");
        return CreateRunResult(input);
    }

    public bool Compare(string projectName)
    {
        var comparisonSetting = Configuration.ComparisonProjects.FirstOrDefault(s => s.Name.ToLower() == projectName.ToLower());
        if (comparisonSetting == null)
        {
            WriteHeadLine($"No project with name {projectName} of the comparison project type found, check that the project exist in {nameof(PowerCommandsConfiguration)}.yaml file.");
            return false;
        }
        try
        {
            var left = new SourceSettingContainer(comparisonSetting.Settings.LeftDataSource);
            left.ConnectionString = Configuration.Secret.DecryptSecret(comparisonSetting.Settings.LeftDataSource.ConnectionString);
            var right = new SourceSettingContainer(comparisonSetting.Settings.RightDataSource);
            right.ConnectionString = Configuration.Secret.DecryptSecret(comparisonSetting.Settings.RightDataSource.ConnectionString);

            var isLeftOk = GetMatrix(left, out IMatrix leftMatrix);
            var isRightOk = GetMatrix(right, out IMatrix rightMatrix);

            var reporter = GetReporter(comparisonSetting.Settings.ReportType);
            var reportFileName = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName, comparisonSetting.Settings.ReportFilePath.PrefixFileTimestamp());

            if (isLeftOk && isRightOk)
            {
                var isEqual = MatrixComparer.IsEqual(comparisonSetting.Settings.ComparisonFields, leftMatrix, rightMatrix, out IMatrix comparedMatrixes);

                reporter.CreateReport(reportFileName, comparedMatrixes, isEqual);
                if (!isEqual)
                {
                    WriteLine($"Differences written to {comparisonSetting.Settings.ReportFilePath.PrefixFileTimestamp()}");
                    ShellService.Service.OpenDirectory(Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName));
                }
                WriteProcessLog(projectName, $"{nameof(Compare)} {nameof(isEqual)} : {isEqual}");
                return isEqual;
            }
            reporter.NonUniqueKeys(reportFileName, leftMatrix, rightMatrix, false);
            ShellService.Service.OpenDirectory(Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName));
            return false;
        }
        catch (Exception e)
        {
            WriteLine($"Couldn't perform glitch detection\n\n{e.Message}");
            return false;
        }
    }
}