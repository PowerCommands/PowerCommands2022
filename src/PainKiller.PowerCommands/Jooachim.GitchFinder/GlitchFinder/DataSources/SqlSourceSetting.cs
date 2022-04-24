using System.Collections.Generic;

namespace GlitchFinder.DataSources
{
    public class SqlSourceSetting : ISourceSetting
    {
        public DataSourceType DataSourceType { get; set; }
        public IEnumerable<string> UniqueKeyFields { get; set; }
        public bool UseEncryption { get; set; }
        public string ConnectionString { get; set; }
        public string Query { get; set; }
        public string QueryFile { get; set; }

        public void SetExample()
        {
            DataSourceType = DataSourceType.MsSql;
            UniqueKeyFields = new List<string> { "PrimaryKeyField1", "PrimaryKeyField2" };
            UseEncryption = false;
            ConnectionString = "Server=servername;Database=databasename;User Id=testUser;Password=#ENCRYPTED#;";
            Query = "select * from table";
            QueryFile = null;
        }
    }
}