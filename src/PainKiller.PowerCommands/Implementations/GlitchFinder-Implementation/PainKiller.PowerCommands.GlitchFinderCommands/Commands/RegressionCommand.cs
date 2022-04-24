using GlitchFinder.Managers;
using GlitchFinder.Matrix;
using GlitchFinder.Matrix.Contracts;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.GlitchFinderCommands.BaseClasses;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[Tags("comparsion|regression")]
[PowerCommand(description: " This is for tracking a single dataset/source over time. You create a baseline, which is stored on file, and then later compare data towards this baseline.",
                arguments: "filename: A valid filename where configuration about the regression test or information about the baseline is stored",
                    qutes: "baseline: if omitted a regression test will be performed, if parameter is baseline a file with baseline data will be created for regression test later",
                  example: "regression \"regressiontest-template.yaml\"|regression baseline \"baseline-data.csv\"")]
public class RegressionCommand : GlitchFinderBaseCommand
{
    public RegressionCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleQuote)) return CreateBadParameterRunResult(input, "A valid file name to existing configuration file must be provided");
        if (input.SingleArgument == "baseline")
        {
            SetBaseline(input.SingleQuote);
            return CreateRunResult(input);
        }
        var isEqual = RegressionTest(input.SingleQuote);
        if (isEqual) WriteHeadLine("No glitches");

        return CreateRunResult(input);
    }
    public bool SetBaseline(string settingsFile)
    {
        try
        {
            var regressionTestSetting = ConfigurationService.Service.Get<RegressionTestSetting>(settingsFile).Configuration;
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
    public bool RegressionTest(string settingsFile)
    {
        var regressionTestSetting = ConfigurationService.Service.Get<RegressionTestSetting>(settingsFile).Configuration;
        try
        {
            var serializedData = File.ReadAllText(regressionTestSetting.BaselineFilePath);

            var baselineMatrix = new GlitchFinder.Matrix.DomainObjects.Matrix(serializedData);
            var isMatrixOk = GetMatrix(regressionTestSetting.SourceSetting, out IMatrix newMatrix);

            var reporter = GetReporter(regressionTestSetting.ReportType);
            var reportFileName = regressionTestSetting.ReportFilePath;

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