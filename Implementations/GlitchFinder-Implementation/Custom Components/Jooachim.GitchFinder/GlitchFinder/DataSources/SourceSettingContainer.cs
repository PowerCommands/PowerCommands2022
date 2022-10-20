using System.Collections.Generic;

namespace GlitchFinder.DataSources
{
    public class SourceSettingContainer : ISourceSetting
    {
        public IEnumerable<string> UniqueKeyFields { get; set; }
        public string ConnectionString { get; set; }
        public string Query { get; set; }
        public string QueryFile { get; set; }
        public bool Header { get; set; }
        public string FilePath { get; set; }
        public string Separator { get; set; }
        public bool Trim { get; set; }
        public List<string> ReplaceHeader { get; set; }
        public DataSourceType DataSourceType { get; set; }
    }
}