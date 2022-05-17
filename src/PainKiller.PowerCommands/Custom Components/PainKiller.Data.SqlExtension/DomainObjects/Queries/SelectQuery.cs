using System.Linq;
using PainKiller.Data.SqlExtension.Contracts;

namespace PainKiller.Data.SqlExtension.DomainObjects.Queries
{
    public class SelectQuery<T> : QueryBase<T>, ISelectQuery where T : new()
    {
        internal string TopClause = "";
        internal string OrderByClause = "";
        internal string AggregateClause = "";
        public override string Sql(string schema)
        {
            var columnsSql = string.Join(',', TableMetadata.Columns.Select(c => c.Name));
            if (!string.IsNullOrEmpty(AggregateClause)) columnsSql = AggregateClause;
            if (!string.IsNullOrEmpty(ContainsClause)) WhereClause = ContainsClause;
            var retVal = $"SELECT {TopClause} {columnsSql} FROM [{schema}].[{TableMetadata.TableName}] {WhereClause} {BetweenClause} {And()} {Or()} {OrderByClause}";
            return retVal;
        }
    }
}