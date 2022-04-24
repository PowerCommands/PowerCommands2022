using GlitchFinder.Managers;
using GlitchFinder.Matrix;
using GlitchFinder.Matrix.Contracts;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.GlitchFinderCommands.BaseClasses;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[Tags("comparsion")]
[PowerCommand(description: "Compare to datasources with each other, find glitches, if any...",
    arguments: "filename: A valid filename where configuration about the comparison is stored",
    example: "comparison \"compare-template.yaml\"")]
public class ComparisonCommand : GlitchFinderBaseCommand
{
    public ComparisonCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleQuote)) return CreateBadParameterRunResult(input, "A valid file name to existing configuration file must be provided");
        var isEqual = Compare(input.SingleQuote);
        if(isEqual) WriteHeadLine("No glitches");
        return CreateRunResult(input);
    }

    public bool Compare(string settingsFile)
    {
        var comparisonSetting = ConfigurationService.Service.Get<ComparisonSetting>(settingsFile).Configuration;
        try
        {
            var isLeftOk = GetMatrix(comparisonSetting.LeftDataSource, out IMatrix leftMatrix);
            var isRightOk = GetMatrix(comparisonSetting.RightDataSource, out IMatrix rightMatrix);

            var reporter = GetReporter(comparisonSetting.ReportType);
            var reportFileName = comparisonSetting.ReportFilePath;

            if (isLeftOk && isRightOk)
            {
                var isEqual = MatrixComparer.IsEqual(comparisonSetting.ComparisonFields, leftMatrix, rightMatrix, out IMatrix comparedMatrixes);

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