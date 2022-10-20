using System.Collections.Generic;
using GlitchFinder.DataSources;
using GlitchFinder.Matrix.Contracts;
using GlitchFinder.Reporters;

namespace GlitchFinder.Managers
{
    public class ComparisonSetting
    {
        public SourceSettingContainer LeftDataSource { get; set; }
        public SourceSettingContainer RightDataSource { get; set; }
        public List<ComparisonField> ComparisonFields { get; set; }
        public ReportType ReportType { get; set; }
        public string ReportFilePath { get; set; }
    }
}
