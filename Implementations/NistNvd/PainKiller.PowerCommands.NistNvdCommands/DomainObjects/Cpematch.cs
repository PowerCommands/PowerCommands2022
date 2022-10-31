namespace PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

public class Cpematch
{
    public bool vulnerable { get; set; }
    public string criteria { get; set; }
    public string matchCriteriaId { get; set; }
    public string versionEndIncluding { get; set; }
    public string versionStartIncluding { get; set; }
    public string versionEndExcluding { get; set; }
}