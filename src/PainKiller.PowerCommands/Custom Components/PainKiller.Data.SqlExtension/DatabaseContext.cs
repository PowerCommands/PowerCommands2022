using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using PainKiller.Data.SqlExtension.Contracts;
using PainKiller.Data.SqlExtension.DomainObjects;
using PainKiller.Data.SqlExtension.DomainObjects.Queries;

namespace PainKiller.Data.SqlExtension
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(DatabaseConfiguration dbConfiguration, bool readOnly = false) : this(dbConfiguration.ConnectionString, dbConfiguration.DefaultSchema, readOnly) { }
        public DatabaseContext(string connectionString, string schema, bool readOnly = false)
        {
            Schema = schema;
            ReadOnly = readOnly;
            _connectionString = connectionString;
        }
        public string Schema { get; }

        public T Find<T>(ISingleQuery singleQuery) where T : new()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = singleQuery.Sql(Schema);
            var retVal = connection.QuerySingleOrDefault<T>(sql);
            return retVal;
        }
        public IEnumerable<T> Select<T>(ISelectQuery selectQuery) where T : new()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = selectQuery.Sql(Schema);
            var retVal = connection.Query<T>(sql);
            return retVal;
        }

        public IEnumerable<T> Where<T>(QueryBase<T> selectQuery) where T : new()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = selectQuery.Sql(Schema);
            var retVal = connection.Query<T>(sql);
            return retVal;
        }
        public TPrimaryKey Insert<T, TPrimaryKey>(IInsertQuery insertQuery)
        {
            if (ReadOnly) throw new ReadOnlyException("This context is read only, insert, update or delete is not allowed");
            using var connection = new SqlConnection(_connectionString);
            var sql = insertQuery.Sql(Schema);
            if (insertQuery.InsertPrimaryKeyValues)
            {
                connection.Query(sql);
                return default;
            }
            var retVal = connection.QuerySingle<TPrimaryKey>(sql);
            return retVal;
        }
        public int Update<T>(QueryBase<T> updateQuery) where T : new()
        {
            if (ReadOnly) throw new ReadOnlyException("This context is read only, insert, update or delete is not allowed");
            using var connection = new SqlConnection(_connectionString);
            var sql = updateQuery.Sql(Schema);
            return connection.Execute(sql);
        }
        public int Delete<T>(QueryBase<T> deleteQuery) where T : new()
        {
            if (ReadOnly) throw new ReadOnlyException("This context is read only, insert, update or delete is not allowed");
            using var connection = new SqlConnection(_connectionString);
            var sql = deleteQuery.Sql(Schema);
            return connection.Execute(sql);
        }

        public bool ReadOnly { get;}
    }
}