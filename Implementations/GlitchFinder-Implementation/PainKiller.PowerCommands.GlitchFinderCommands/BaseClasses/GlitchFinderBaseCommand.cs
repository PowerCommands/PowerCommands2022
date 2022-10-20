using GlitchFinder.Contracts;
using GlitchFinder.DataSources;
using GlitchFinder.Matrix.Contracts;
using GlitchFinder.Reporters;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.BaseClasses;

public abstract class GlitchFinderBaseCommand : CommandBase<PowerCommandsConfiguration>
{
    protected string ProjectPath = "";
    protected string ComparisonConfigFileName = "comparison.yaml";
    protected string RegressionTestConfigFileName = "regression-test.yaml";

    protected GlitchFinderBaseCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration)
    {
        SqlImport.SetDecryptSecretFunction(Configuration.Secret.DecryptSecret);
    }
    protected bool GetMatrix(SourceSettingContainer setting, out IMatrix matrix)
    {
        switch (setting.DataSourceType)
        {
            case DataSourceType.MsSql:
                return new SqlImport().TryImport(setting, out matrix);
            case DataSourceType.CsvFile:
                return new CsvFileImport().TryImport(setting, out matrix);
            default:
                throw new NotImplementedException($"SourceType {setting.DataSourceType} is not implemented");
        }
    }
    protected IGlitchReport GetReporter(ReportType reportType)
    {
        switch (reportType)
        {
            case ReportType.Html:
                return new HtmlReport();
            case ReportType.Csv:
                return new CsvReport();
            case ReportType.Excel:
                return new ExcelReport();
            default:
                throw new NotImplementedException($"ReportType {reportType} is not implemented");
        }
    }
}