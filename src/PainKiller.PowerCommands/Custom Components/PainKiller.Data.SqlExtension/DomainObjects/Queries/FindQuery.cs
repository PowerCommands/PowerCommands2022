using System.Linq;
using PainKiller.Data.SqlExtension.Contracts;
using PainKiller.Data.SqlExtension.Extensions;

namespace PainKiller.Data.SqlExtension.DomainObjects.Queries
{
    public class FindQuery<T,TPrimaryKey> : ISingleQuery where T : new()
    {
        private readonly string _whereClause = "";
        private readonly TableMetadata TableMetadata;

        public FindQuery(TPrimaryKey primaryKeyValue)
        {
            TableMetadata = new T().GeTableMetadata();
            _whereClause = $"WHERE [{TableMetadata.PrimaryKeys}] = {primaryKeyValue.ToSqlFormattedValue()}";
        }

        public string Sql(string schema)
        {
            var retVal = $"SELECT {string.Join(',', TableMetadata.Columns.Select(c => c.Name))} FROM [{schema}].[{TableMetadata.TableName}] {_whereClause}";
            return retVal;
        }
    }
}