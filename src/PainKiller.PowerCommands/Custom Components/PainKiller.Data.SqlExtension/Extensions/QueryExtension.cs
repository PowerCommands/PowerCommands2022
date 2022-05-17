using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using PainKiller.Data.SqlExtension.DomainObjects.Queries;
using PainKiller.Data.SqlExtension.Enums;

namespace PainKiller.Data.SqlExtension.Extensions
{
    public static class QueryExtension
    {
        public static QueryBase<TSelectItem> Between<TSelectItem, TBetween>(this QueryBase<TSelectItem> selectQuery, Expression<Func<TSelectItem, TBetween>> betweenExpression1, Expression<Func<TSelectItem, TBetween>> betweenExpression2) where TSelectItem : new()
        {
            var bodyExpression1 = (BinaryExpression)betweenExpression1.Body;
            var name = bodyExpression1.Left.GetName();
            var value1 = bodyExpression1.Right.GetSqlValue();

            var bodyExpression2 = (BinaryExpression)betweenExpression2.Body;
            var value2 = bodyExpression2.Right.GetSqlValue();

            var where = string.IsNullOrEmpty(selectQuery.WhereClause) ? "WHERE" : "AND";
            selectQuery.BetweenClause = $"{where} {name} BETWEEN {value1} AND {value2}";
            return selectQuery;
        }
        public static QueryBase<TSelectItem> Contains<TSelectItem>(this QueryBase<TSelectItem> selectQuery, Expression<Func<TSelectItem, bool>> queryExpression) where TSelectItem : new()
        {
            return Like(selectQuery, queryExpression);
        }

        public static QueryBase<TSelectItem> StartsWith<TSelectItem>(this QueryBase<TSelectItem> selectQuery, Expression<Func<TSelectItem, bool>> queryExpression) where TSelectItem : new()
        {
            return Like(selectQuery, queryExpression, endsWidth: "");
        }

        public static QueryBase<TSelectItem> EndsWith<TSelectItem>(this QueryBase<TSelectItem> selectQuery, Expression<Func<TSelectItem, bool>> queryExpression) where TSelectItem : new()
        {
            return Like(selectQuery, queryExpression, startsWith: "");
        }
        private static QueryBase<TSelectItem> Like<TSelectItem>(this QueryBase<TSelectItem> selectQuery, Expression<Func<TSelectItem, bool>> queryExpression, string startsWith = "%", string endsWidth = "%") where TSelectItem : new()
        {
            var where = string.IsNullOrEmpty(selectQuery.BetweenClause) ? "WHERE" : "AND";
            selectQuery.WhereClause = $"{where} {queryExpression.ToSqlConditionalClause()}";
            var clause = new Clause(selectQuery.WhereClause);
            selectQuery.ContainsClause = $"{where} {clause.LeftSideArgument} LIKE '{endsWidth}{clause.RightSideArgument.Replace("'", "")}{startsWith}'";
            return selectQuery;
        }
        public static QueryBase<TSelectItem> Where<TSelectItem>(this QueryBase<TSelectItem> selectQuery, Expression<Func<TSelectItem, bool>> queryExpression) where TSelectItem : new()
        {
            var where = string.IsNullOrEmpty(selectQuery.BetweenClause) ? "WHERE" : "AND";
            selectQuery.WhereClause = $"{where} {queryExpression.ToSqlConditionalClause()}";
            return selectQuery;
        }

        public static QueryBase<TSelectItem> And<TSelectItem>(this QueryBase<TSelectItem> selectQuery, Expression<Func<TSelectItem, bool>> queryExpression) where TSelectItem : new()
        {
            var and = queryExpression.ToSqlConditionalClause();
            selectQuery.AndClauses.Add(and);
            return selectQuery;
        }

        public static QueryBase<TSelectItem> Or<TSelectItem>(this QueryBase<TSelectItem> selectQuery, Expression<Func<TSelectItem, bool>> queryExpression) where TSelectItem : new()
        {
            var or = queryExpression.ToSqlConditionalClause();
            selectQuery.OrClauses.Add(or);
            return selectQuery;
        }

        public static SelectQuery<TSelectItem> OrderBy<TSelectItem, TOrderBy>(this QueryBase<TSelectItem> query, Expression<Func<TSelectItem, TOrderBy>> orderByExpression, SortOrder sortOrder = SortOrder.Ascending) where TSelectItem : new()
        {
            var selectQuery = (SelectQuery<TSelectItem>)query;
            //var bodyExpression = (BinaryExpression)orderByExpression.Body;
            var orderBy = orderByExpression.Body.ToString().Split('.').Last();
            //var orderBy = bodyExpression.Right.GetName();
            var sortBy = sortOrder == SortOrder.Ascending ? "ASC" : "DESC";
            selectQuery.OrderByClause = $"ORDER BY {orderBy} {sortBy}";
            return selectQuery;
        }

        public static SelectQuery<TSelectItem> Take<TSelectItem>(this SelectQuery<TSelectItem> selectQuery, int take) where TSelectItem : new()
        {
            selectQuery.TopClause = $"TOP {take}";
            return selectQuery;
        }

        public static SelectQuery<TSelectItem> Take<TSelectItem>(this QueryBase<TSelectItem> query, int take) where TSelectItem : new()
        {
            var selectQuery = (SelectQuery<TSelectItem>)query;
            selectQuery.TopClause = $"TOP {take}";
            return selectQuery;
        }
        public static SelectQuery<TSelectItem> Aggregate<TSelectItem, TAggregate>(this SelectQuery<TSelectItem> query, Expression<Func<TSelectItem, TAggregate>> aggregateExpression, AggregateType aggregateType) where TSelectItem : new()
        {
            var selectQuery = (SelectQuery<TSelectItem>)query;
            var aggregateColumn = aggregateExpression.Body.ToString().Split('.').Last();
            var aggregateSql = $"{aggregateType.ToString().ToUpper()}({aggregateColumn}) AS {aggregateColumn}";

            selectQuery.AggregateClause = aggregateSql;
            return selectQuery;
        }

    }
}