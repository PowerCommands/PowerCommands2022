using System.Collections.Generic;

namespace GlitchFinder.DataSources
{
    public class SourceSettingContainer : ISourceSetting
    {
        public SourceSettingContainer(){}
        public SourceSettingContainer(SourceSettingContainer item)
        {
            UniqueKeyFields = item.UniqueKeyFields;
            UseEncryption = item.UseEncryption;
            ConnectionString = item.ConnectionString;
            Query = item.Query;
            QueryFile = item.QueryFile;
            Header = item.Header;
            FilePath = item.FilePath;
            Separator = item.Separator;
            Trim = item.Trim;
            ReplaceHeader = item.ReplaceHeader;
            DataSourceType = item.DataSourceType;
        }
        public IEnumerable<string> UniqueKeyFields { get; set; }
        public bool UseEncryption { get; set; }
        public string ConnectionString { get; set; }
        public string Query { get; set; }
        public string QueryFile { get; set; }
        public bool Header { get; set; }
        public string FilePath { get; set; }
        public string Separator { get; set; }
        public bool Trim { get; set; }
        public List<string> ReplaceHeader { get; set; }
        public DataSourceType DataSourceType { get; set; }

        public void SetExample()
        {
            throw new System.NotImplementedException();
        }
    }
}