using System.Collections.Generic;

namespace GlitchFinder.DataSources
{
    public class CsvFileSourceSetting : ISourceSetting
    {
        public DataSourceType DataSourceType { get; set; }
        public IEnumerable<string> UniqueKeyFields { get; set; }
        public bool Header { get; set; }
        public bool Trim { get; set; }
        public List<string> ReplaceHeader { get; set; }
        public string FilePath { get; set; }
        public string Separator { get; set; }


        public void SetExample()
        {
            DataSourceType = DataSourceType.CsvFile;
            UniqueKeyFields = new List<string> { "PrimaryKeyField1", "PrimaryKeyField2" };
            Header = true;
            Trim = false;
            ReplaceHeader = new List<string> { "AlternateFieldName1", "AlternateFieldName2" };
            FilePath = "#PATH_TO_FILE#";
            Separator = "\t";
        }
    }
}