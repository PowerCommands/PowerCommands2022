namespace PainKiller.Data.SqlExtension.DomainObjects
{
    public class DatabaseConfiguration
    {
        public DatabaseConfiguration(string connectionString, string defaultSchema)
        {
            ConnectionString = connectionString;
            DefaultSchema = defaultSchema;
        }
        public string ConnectionString { get; }
        public string DefaultSchema { get; }
    }
}