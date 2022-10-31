namespace PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

public class Cve
{
    public string id { get; set; }
    public string sourceIdentifier { get; set; }
    public DateTime published { get; set; }
    public DateTime lastModified { get; set; }
    public string vulnStatus { get; set; }
    public Description[] descriptions { get; set; }
    public Metrics metrics { get; set; }
    public Weakness[] weaknesses { get; set; }
    public Configuration[] configurations { get; set; }
    public Reference[] references { get; set; }
    public string cisaExploitAdd { get; set; }
    public string cisaActionDue { get; set; }
    public string cisaRequiredAction { get; set; }
    public string cisaVulnerabilityName { get; set; }
    public string evaluatorSolution { get; set; }
    public string evaluatorImpact { get; set; }
    public string evaluatorComment { get; set; }
    public Vendorcomment[] vendorComments { get; set; }
}