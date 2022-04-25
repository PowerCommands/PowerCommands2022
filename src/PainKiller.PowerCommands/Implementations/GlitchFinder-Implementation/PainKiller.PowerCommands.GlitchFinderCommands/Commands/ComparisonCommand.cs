using GlitchFinder.Matrix;
using GlitchFinder.Matrix.Contracts;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.GlitchFinderCommands.BaseClasses;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[Tags("comparsion")]
[PowerCommand(description: "Compare to datasources with each other, find glitches, if any...",
    arguments: "project name: Existing comparison project with configuration information about the comparison",
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
            comparisonSetting.Settings.LeftDataSource.ConnectionString = Configuration.Secret.DecryptSecret(comparisonSetting.Settings.LeftDataSource.ConnectionString); 
            comparisonSetting.Settings.RightDataSource.ConnectionString = Configuration.Secret.DecryptSecret(comparisonSetting.Settings.RightDataSource.ConnectionString);

            var isLeftOk = GetMatrix(comparisonSetting.Settings.LeftDataSource, out IMatrix leftMatrix);
            var isRightOk = GetMatrix(comparisonSetting.Settings.RightDataSource, out IMatrix rightMatrix);

            var reporter = GetReporter(comparisonSetting.Settings.ReportType);
            var reportFileName = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName, comparisonSetting.Settings.ReportFilePath.PrefixFileTimestamp());

            if (isLeftOk && isRightOk)
            {
                var isEqual = MatrixComparer.IsEqual(comparisonSetting.Settings.ComparisonFields, leftMatrix, rightMatrix, out IMatrix comparedMatrixes);

                reporter.CreateReport(reportFileName, comparedMatrixes, isEqual);
                if (!isEqual) WriteLine($"Differences written to {reportFileName}");

                return isEqual;
            }
            reporter.NonUniqueKeys(reportFileName, leftMatrix, rightMatrix, false);
            return false;
        }
        catch (Exception e)
        {
            WriteLine($"Couldn't perform glitch detection\n\n{e.Message}");
            return false;
        }
    }
}