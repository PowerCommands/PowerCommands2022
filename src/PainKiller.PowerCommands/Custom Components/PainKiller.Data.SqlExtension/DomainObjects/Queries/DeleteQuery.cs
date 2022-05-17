using System;
using PainKiller.Data.SqlExtension.Contracts;

namespace PainKiller.Data.SqlExtension.DomainObjects.Queries
{
    public class DeleteQuery<T> : QueryBase<T>, IDeleteQuery where T: new()
    {
        public override string Sql(string schema)
        {
            if (string.IsNullOrEmpty(WhereClause) && string.IsNullOrEmpty(BetweenClause)) throw new ArgumentNullException($"Where statement is missing, that is not allowed");
            var retVal = $"DELETE [{schema}].[{TableMetadata.TableName}] {WhereClause} {BetweenClause} {And()} {Or()}";
            return retVal;
        }
    }
}