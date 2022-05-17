using System.Collections.Generic;
using PainKiller.Data.SqlExtension.Extensions;

namespace PainKiller.Data.SqlExtension.DomainObjects.Queries
{
    public abstract class QueryBase<T> where T : new()
    {
        protected readonly TableMetadata TableMetadata;
        internal readonly List<string> AndClauses = new List<string>();
        internal readonly List<string> OrClauses = new List<string>();

        internal string WhereClause = "";
        internal string BetweenClause = "";
        internal string ContainsClause = "";
        protected QueryBase() { TableMetadata = new T().GeTableMetadata(); }
        protected QueryBase(T item) { TableMetadata = item.GeTableMetadata(); }

        protected string And()
        {
            var clauses = string.Join(" AND ", AndClauses);
            var and = string.IsNullOrEmpty(clauses) ? "" : $"AND {clauses}";
            return and;
        }
        protected string Or()
        {
            var clauses = string.Join(" OR ", OrClauses);
            var or = string.IsNullOrEmpty(clauses) ? "" : $"OR {clauses}";
            return or;
        }

        public virtual string Sql(string schema) { return schema; }
    }
}