namespace GlitchFinder.DataSources
{
    public static class ConfigurationExtension
    {
        public static CsvFileSourceSetting ToCsvFileSourceSetting(this SourceSettingContainer source) => new() {
            UniqueKeyFields = source.UniqueKeyFields,
            FilePath = source.FilePath,
            Separator = source.Separator,
            Header = source.Header,
            ReplaceHeader = source.ReplaceHeader,
            Trim = source.Trim };

        public static SourceSettingContainer ToSourceSettingContainer(this CsvFileSourceSetting source) => new() {
            DataSourceType = DataSourceType.CsvFile,
            UniqueKeyFields = source.UniqueKeyFields,
            FilePath = source.FilePath,
            Separator = source.Separator,
            Header = source.Header,
            ReplaceHeader = source.ReplaceHeader,
            Trim = source.Trim
        };

        public static SqlSourceSetting ToDatabaseSourceSetting(this SourceSettingContainer source) => new() { 
            ConnectionString = source.ConnectionString, 
            Query = source.Query, 
            QueryFile = source.QueryFile, 
            UniqueKeyFields = source.UniqueKeyFields};

        public static SourceSettingContainer ToSourceSettingContainer(this SqlSourceSetting source) => new()
        {
            DataSourceType = DataSourceType.MsSql,
            ConnectionString = source.ConnectionString,
            Query = source.Query,
            QueryFile = source.QueryFile,
            UniqueKeyFields = source.UniqueKeyFields
        };
    };
}