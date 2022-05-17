using System;
using System.Linq;
using PainKiller.Data.SqlExtension.Contracts;
using PainKiller.Data.SqlExtension.Extensions;

namespace PainKiller.Data.SqlExtension.DomainObjects.Queries
{
    public class UpdateQuery<T> : QueryBase<T>, IUpdateQuery where T:new()
    {
        private readonly TableMetadata _tableMetadata;
        public UpdateQuery(T updateItem)
        {
            _tableMetadata = updateItem.GeTableMetadata();
        }
        public override string Sql(string schema)
        {
            if (string.IsNullOrEmpty(WhereClause) && string.IsNullOrEmpty(BetweenClause)) throw new ArgumentNullException($"Where statement is missing, that is not allowed");
            var columns = _tableMetadata.Columns.Where(c => !c.IsPrimaryKey).Select(c => $"{c.Name} = {c.Value}").ToArray();
            var updateStament = $"SET {string.Join(',', columns)}";

            var retVal = $"UPDATE [{schema}].[{_tableMetadata.TableName}] {updateStament} {WhereClause} {BetweenClause} {And()} {Or()}";
            return retVal;
        }
    }
}