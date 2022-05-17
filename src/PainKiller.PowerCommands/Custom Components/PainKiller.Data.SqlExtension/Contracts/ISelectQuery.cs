namespace PainKiller.Data.SqlExtension.Contracts
{
    public interface ISelectQuery
    {
        string Sql(string schema);
    }
}