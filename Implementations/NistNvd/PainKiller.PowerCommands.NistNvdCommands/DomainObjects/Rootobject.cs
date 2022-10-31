namespace PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

public class Rootobject
{
    public int resultsPerPage { get; set; }
    public int startIndex { get; set; }
    public int totalResults { get; set; }
    public string format { get; set; }
    public string version { get; set; }
    public DateTime timestamp { get; set; }
    public Vulnerability[] vulnerabilities { get; set; }
}