using System;

namespace PainKiller.Data.SqlExtension.DomainObjects
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class TableMetadataAttribute : Attribute
    {
        /// <summary>
        /// Metadatainformation about the table
        /// </summary>
        public TableMetadata TableInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">Name of the origin table or view in the database</param>
        public TableMetadataAttribute(string tableName)
        {
            TableInfo = new TableMetadata(tableName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">Name of the origin table or view in the database</param>
        /// <param name="primaryKeys">Primary key(s) is used on insert, update, delete</param>
        public TableMetadataAttribute(string tableName, string primaryKeys)
        {
            TableInfo = new TableMetadata(tableName, primaryKeys);
        }
    }
}