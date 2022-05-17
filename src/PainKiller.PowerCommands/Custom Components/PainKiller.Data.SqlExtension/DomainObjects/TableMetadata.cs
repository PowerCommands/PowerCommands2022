namespace PainKiller.Data.SqlExtension.DomainObjects
{
    /// <summary>
    /// Metadatainformation about the table
    /// </summary>
    public class TableMetadata
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">Name of the origin table or view in the database</param>
        public TableMetadata(string tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">Name of the origin table or view in the database</param>
        /// <param name="primaryKeys">Primary key(s) is used on insert, update and delete querys</param>
        public TableMetadata(string tableName, string primaryKeys)
        {
            TableName = tableName;
            PrimaryKeys = $"{primaryKeys}";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">Name of the origin table or view in the database</param>
        /// <param name="primaryKeys">Primary key(s) is used on insert, update and delete querys</param>
        /// <param name="columns">column metadata</param>
        public TableMetadata(string tableName, string primaryKeys, SqlColumn[] columns)
        {
            TableName = tableName;
            PrimaryKeys = primaryKeys;
            Columns = columns;
        }

        public SqlColumn[] Columns { get; }

        /// <summary>
        /// Name of the origin table or view in the database
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Primary key(s) is used on insert, update and delete querys
        /// </summary>
        public string PrimaryKeys { get; } = "";
    }
}