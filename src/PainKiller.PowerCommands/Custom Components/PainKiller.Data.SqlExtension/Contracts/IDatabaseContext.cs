using System.Collections.Generic;
using PainKiller.Data.SqlExtension.DomainObjects.Queries;

namespace PainKiller.Data.SqlExtension.Contracts
{
    public interface IDatabaseContext : IRepository
    {
        T Find<T>(ISingleQuery singleQuery) where T : new();
        IEnumerable<T> Select<T>(ISelectQuery selectQuery) where T : new();
        IEnumerable<T> Where<T>(QueryBase<T> selectQuery) where T : new();
        TPrimaryKey Insert<T, TPrimaryKey>(IInsertQuery insertQuery);
        int Update<T>(QueryBase<T> updateQuery) where T : new();
        int Delete<T>(QueryBase<T> deleteQuery) where T : new();
        bool ReadOnly { get;}
    }
}