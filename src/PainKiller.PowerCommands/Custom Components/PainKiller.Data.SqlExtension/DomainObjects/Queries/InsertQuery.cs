using System.Linq;
using PainKiller.Data.SqlExtension.Contracts;
using PainKiller.Data.SqlExtension.Extensions;

namespace PainKiller.Data.SqlExtension.DomainObjects.Queries
{
    public class InsertQuery<T> : IInsertQuery
    {
        private readonly TableMetadata _tableMetadata;

        public InsertQuery(T insertItem) : this(insertItem, false, false) { }
        public InsertQuery(T insertItem, bool insertPrimaryKeyValues) : this(insertItem, insertPrimaryKeyValues, false) { }
        public InsertQuery(T insertItem, bool insertPrimaryKeyValues, bool enableTriggers)
        {
            InsertPrimaryKeyValues = insertPrimaryKeyValues;
            EnableTriggers = enableTriggers;
            _tableMetadata = insertItem.GeTableMetadata();
        }

        public bool InsertPrimaryKeyValues { get; }
        /// <summary>
        /// DML statement cannot have any enabled triggers if the statement contains an OUTPUT clause without INTO clause, EnableTriggers will remove OUTPUT from insert statement
        /// </summary>
        public bool EnableTriggers { get; set; }

        public string Sql(string schema)
        {
            var columns = _tableMetadata.Columns.Where(c => !c.IsPrimaryKey || InsertPrimaryKeyValues).ToArray();
            var primaryKeyColumns = _tableMetadata.Columns.Where(c => c.IsPrimaryKey).Select(c => $"INSERTED.{c.Name}").ToList();
            var columnNames = string.Join(',', columns.OrderBy(c => c.Name).Select(c => c.Name));
            var columnValues = string.Join(',', columns.OrderBy(c => c.Name).Select(c => c.Value));

            var output = "";
            if (primaryKeyColumns.Count > 0 && !EnableTriggers) output = $"OUTPUT {string.Join(',', primaryKeyColumns)}";

            var retVal = $"INSERT INTO[{schema}].[{_tableMetadata.TableName}] ({columnNames}) {output} VALUES ({columnValues})";
            return retVal;
        }
    }
}