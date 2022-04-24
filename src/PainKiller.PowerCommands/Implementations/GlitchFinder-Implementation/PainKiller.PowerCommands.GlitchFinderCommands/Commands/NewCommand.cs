using GlitchFinder.DataSources;
using GlitchFinder.Managers;
using GlitchFinder.Matrix.Contracts;
using GlitchFinder.Reporters;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[Tags("configuration|help")]
[PowerCommand(description: "Creates a new config files from a template",
                arguments: "type: Configuration type, values comparsion or regression",
                    qutes: "filename: A valid filename",
                  example: "initialize comparsion \"compare-template.yaml\"|initialize regression \"regression-template.yaml\"")]
public class InitializeCommand : CommandBase<PowerCommandsConfiguration>
{
    public InitializeCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.SingleQuote)) return CreateBadParameterRunResult(input, "A valid file name to new config must be provided");
        if(string.IsNullOrEmpty(input.SingleArgument)) return CreateBadParameterRunResult(input, "You must provide witch config file to create, either comparsion or regression");
        if(input.SingleArgument == "comparsion") NewComparison(input.SingleQuote);
        if(input.SingleArgument == "regression") NewRegressionTest(input.SingleQuote);
        return CreateRunResult(input);
    }

    public void NewComparison(string newSettingsFileName)
    {
        var comparisonSetting = new ComparisonSetting
        {
            LeftDataSource = GetDefaultSettings(DataSourceType.MsSql),
            RightDataSource = GetDefaultSettings(DataSourceType.MsSql),
            ReportType = ReportType.Html,
            ReportFilePath = "#PATH_TO_FILE#",
            ComparisonFields = new List<ComparisonField> { new() { LeftFieldName = "LeftMatrixField", RightFieldName = "RightMatrixField" } }
        };
        ConfigurationService.Service.SaveChanges(comparisonSetting, $"{newSettingsFileName}");
        WriteLine($"A new configuration template \"{newSettingsFileName}\" has been created");
    }

    public void NewRegressionTest(string newSettingsFileName)
    {
        var regressionTestSettings = new RegressionTestSetting()
        {
            BaselineFilePath = "#PATH_TO_FILE#",
            ReportFilePath = "#PATH_TO_FILE#",
            ReportType = ReportType.Csv,
            ComparisonFields = new List<string> { "Field1", "Field2" },
            SourceSetting = GetDefaultSettings(DataSourceType.MsSql)
        };
        ConfigurationService.Service.SaveChanges(regressionTestSettings, $"{newSettingsFileName}");
        WriteLine($"A new configuration template \"{newSettingsFileName}\" has been created");
    }
    private SourceSettingContainer GetDefaultSettings(DataSourceType dataSourceType)
    {
        switch (dataSourceType)
        {
            case DataSourceType.MsSql:
                var sqlSourceSetting = new SqlSourceSetting();
                sqlSourceSetting.SetExample();
                return sqlSourceSetting.ToSourceSettingContainer();
            case DataSourceType.CsvFile:
            default:
                var csvFileSourceSetting = new CsvFileSourceSetting();
                csvFileSourceSetting.SetExample();
                return csvFileSourceSetting.ToSourceSettingContainer();
        }
    }
}