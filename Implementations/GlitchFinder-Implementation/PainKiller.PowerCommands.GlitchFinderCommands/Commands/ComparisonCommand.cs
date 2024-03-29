﻿using GlitchFinder.Managers;
using GlitchFinder.Matrix;
using GlitchFinder.Matrix.Contracts;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.GlitchFinderCommands.BaseClasses;
using PainKiller.PowerCommands.GlitchFinderCommands.Configuration;

namespace PainKiller.PowerCommands.GlitchFinderCommands.Commands;

[PowerCommandDesign(description: "Compare to datasources with each other, find glitches, if any...",
                      arguments: "!project:<name>",
                        example: "comparison \"My sample project\"")]
public class ComparisonCommand : GlitchFinderBaseCommand
{
    public ComparisonCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (string.IsNullOrEmpty(Input.SingleQuote)) return BadParameterError("You must provide a project name");
        var isEqual = Compare(Input.SingleQuote);
        if(isEqual) WriteHeadLine("No glitches");
        return Ok();
    }

    public bool Compare(string projectName)
    {
        if (Configuration.ComparisonProjects.FirstOrDefault(s => s.Name.ToLower() == projectName.ToLower()) == null)
        {
            WriteHeadLine($"No project with name {projectName} of the comparison project type found, check that the project exist in {nameof(PowerCommandsConfiguration)}.yaml file.");
            return false;
        }

        ProjectPath = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName);
        var comparisonProject = ConfigurationService.Service.Get<ComparisonSetting>(Path.Combine(ProjectPath, $"{ComparisonConfigFileName}"));
        try
        {
            var isLeftOk = GetMatrix(comparisonProject.Configuration.LeftDataSource, out IMatrix leftMatrix);
            var isRightOk = GetMatrix(comparisonProject.Configuration.RightDataSource, out IMatrix rightMatrix);

            var reporter = GetReporter(comparisonProject.Configuration.ReportType);
            var reportFileName = Path.Combine(AppContext.BaseDirectory, Configuration.ProjectsRelativePath, projectName, comparisonProject.Configuration.ReportFilePath.PrefixFileTimestamp());

            if (isLeftOk && isRightOk)
            {
                var isEqual = MatrixComparer.IsEqual(comparisonProject.Configuration.ComparisonFields, leftMatrix, rightMatrix, out IMatrix comparedMatrixes);

                reporter.CreateReport(reportFileName, comparedMatrixes, isEqual);
                if (!isEqual)
                {
                    WriteLine($"Differences written to {comparisonProject.Configuration.ReportFilePath.PrefixFileTimestamp()}");
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