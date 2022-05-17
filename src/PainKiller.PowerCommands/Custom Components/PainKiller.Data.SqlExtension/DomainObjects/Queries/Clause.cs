namespace PainKiller.Data.SqlExtension.DomainObjects.Queries
{
    public class Clause
    {
        public Clause(string clause)
        {
            var query = clause.Split(' ');
            QueryType = query[0];
            LeftSideArgument = query[1];
            Operator = query[2];
            RightSideArgument = query[3];
        }
        public string QueryType { get; }
        public string LeftSideArgument { get; }
        public string RightSideArgument { get; }
        public string Operator { get; }
    }
}