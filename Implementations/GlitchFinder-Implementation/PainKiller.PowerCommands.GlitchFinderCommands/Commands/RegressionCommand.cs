using GlitchFinder.Managers;
using GlitchFinder.Matrix;
using GlitchFinder.Matrix.Contracts;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.GlitchFinderCommands.BaseClasses;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[PowerCommandDesign(description: "This is for tracking a single dataset/source over time.\nYou create a baseline, which is stored on file, and then later compare data towards this baseline.",
                      arguments: "baseline",
                         quotes: "!project:<name>",
                        example: "regression \"regression sample project\"|regression baseline \"regression sample project\"")]
public class RegressionCommand : GlitchFinderBaseCommand
{
    public RegressionCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (string.IsNullOrEmpty(Input.SingleQuote)) return BadParameterError("A valid project name to existing configuration file must be provided");
        var projectName = Input.SingleQuote;
        
        if (Configuration.RegressionProjects.FirstOrDefault(s => s.Name.ToLower() == projectName.ToLower()) == null) return BadParameterError($"No project with name {projectName} of the regression project type found, check that the project exist in {nameof(PowerCommandsConfiguration)}.yaml file.");

        ProjectPath = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName);
        var config = ConfigurationService.Service.Get<RegressionTestSetting>(Path.Combine(ProjectPath, $"{RegressionTestConfigFileName}")).Configuration;

        if (Input.SingleArgument == "baseline")
        {
            SetBaseline(config, projectName);
            return Ok();
        }
        var isEqual = RegressionTest(config, projectName);
        if (isEqual) WriteHeadLine("No glitches");

        return Ok();
    }
    public bool SetBaseline(RegressionTestSetting config, string projectName)
    {
        try
        {
            GetMatrix(config.SourceSetting, out IMatrix baselineMatrix);

            var serialized = ((GlitchFinder.Matrix.DomainObjects.Matrix)baselineMatrix).Serialize();
            File.WriteAllText(GetFileName(projectName, config.BaselineFilePath), serialized);
            WriteLine($"Baseline file \"{config.BaselineFilePath}\" created.");
            WriteProcessLog(projectName, $"{nameof(SetBaseline)} {config.BaselineFilePath}");
            return true;
        }
        catch (Exception e)
        {
            WriteLine($"Couldn't perform baselining\n\n{e.Message}");
            return false;
        }
    }
    public bool RegressionTest(RegressionTestSetting config, string projectName)
    {
        try
        {
            var serializedData = File.ReadAllText(GetFileName(projectName, config.BaselineFilePath));

            var baselineMatrix = new GlitchFinder.Matrix.DomainObjects.Matrix(serializedData);
            var isMatrixOk = GetMatrix(config.SourceSetting, out IMatrix newMatrix);

            var reporter = GetReporter(config.ReportType);
            var reportFileName = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName, config.ReportFilePath.PrefixFileTimestamp());

            if (isMatrixOk)
            {
                var comparisonFields = config.ComparisonFields.Select(c => new ComparisonField { LeftFieldName = c, RightFieldName = c }).ToList();
                var isEqual = MatrixComparer.IsEqual(comparisonFields, baselineMatrix, newMatrix, out IMatrix comparedMatrixes);

                reporter.CreateReport(reportFileName, comparedMatrixes, isEqual);
                if (!isEqual)
                {
                    WriteLine($"Differences written to {reportFileName}");
                    ShellService.Service.OpenDirectory(Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName));
                }
                WriteProcessLog(projectName, $"{nameof(RegressionTest)} {nameof(isEqual)}: {isEqual}");
                return isEqual;
            }
            reporter.NonUniqueKeys(reportFileName, baselineMatrix, newMatrix, false);
            return false;
        }
        catch (Exception e)
        {
            WriteLine($"Couldn't perform regression test\n\n{e.Message}");
            return false;
        }
    }
    private string GetFileName(string projectName, string fileName) => Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName, fileName);
}