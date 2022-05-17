namespace PainKiller.Data.SqlExtension.Contracts
{
    public interface ISingleQuery
    {
        string Sql(string schema);
    }
}