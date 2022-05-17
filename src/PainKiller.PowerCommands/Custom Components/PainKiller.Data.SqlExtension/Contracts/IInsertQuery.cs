namespace PainKiller.Data.SqlExtension.Contracts
{
    public interface IInsertQuery
    {
        bool InsertPrimaryKeyValues { get; }
        public bool EnableTriggers { get; }
        string Sql(string schema);
    }
}