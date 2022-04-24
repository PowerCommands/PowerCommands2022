using GlitchFinder.Matrix;
using GlitchFinder.Matrix.Contracts;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.GlitchFinderCommands.BaseClasses;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[Tags("comparsion|regression")]
[PowerCommand(description: " This is for tracking a single dataset/source over time. You create a baseline, which is stored on file, and then later compare data towards this baseline.",
                arguments: "project name: Existing regression test project with configuration information about the regression test",
                    qutes: "baseline: if omitted a regression test will be performed, if parameter is baseline a file with baseline data will be created for regression test later",
                  example: "regression \"regression sample project\"|regression baseline \"regression sample project\"")]
public class RegressionCommand : GlitchFinderBaseCommand
{
    public RegressionCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleQuote)) return CreateBadParameterRunResult(input, "A valid file name to existing configuration file must be provided");
        var projectName = input.SingleQuote;
        var project = Configuration.RegressionProjects.FirstOrDefault(s => s.Name.ToLower() == projectName.ToLower());
        
        if (project == null) return CreateBadParameterRunResult(input, $"No project with name {projectName} of the regression project type found, check that the project exist in {nameof(PowerCommandsConfiguration)}.yaml file.");
        
        if (input.SingleArgument == "baseline")
        {
            SetBaseline(project);
            return CreateRunResult(input);
        }
        var isEqual = RegressionTest(project);
        if (isEqual) WriteHeadLine("No glitches");

        return CreateRunResult(input);
    }
    public bool SetBaseline(RegressionProject project)
    {
        try
        {
            var regressionTestSetting = project.Settings;

            GetMatrix(regressionTestSetting.SourceSetting, out IMatrix baselineMatrix);

            var serialized = ((GlitchFinder.Matrix.DomainObjects.Matrix)baselineMatrix).Serialize();
            File.WriteAllText(regressionTestSetting.BaselineFilePath, serialized);
            WriteLine($"Baseline file \"{regressionTestSetting.BaselineFilePath}\" created.");
            return true;
        }
        catch (Exception e)
        {
            WriteLine($"Couldn't perform baselining\n\n{e.Message}");
            return false;
        }
    }
    public bool RegressionTest(RegressionProject project)
    {
        var regressionTestSetting = project.Settings;
        try
        {
            var serializedData = File.ReadAllText(regressionTestSetting.BaselineFilePath);

            var baselineMatrix = new GlitchFinder.Matrix.DomainObjects.Matrix(serializedData);
            var isMatrixOk = GetMatrix(regressionTestSetting.SourceSetting, out IMatrix newMatrix);

            var reporter = GetReporter(regressionTestSetting.ReportType);
            var reportFileName = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, project.Name, regressionTestSetting.ReportFilePath.PrefixFileTimestamp());

            if (isMatrixOk)
            {
                var comparisonFields = regressionTestSetting.ComparisonFields.Select(c => new ComparisonField { LeftFieldName = c, RightFieldName = c }).ToList();
                var isEqual = MatrixComparer.IsEqual(comparisonFields, baselineMatrix, newMatrix, out IMatrix comparedMatrixes);

                reporter.CreateReport(reportFileName, comparedMatrixes, isEqual);
                if (!isEqual)
                    WriteLine($"Differences written to {reportFileName}");
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
}