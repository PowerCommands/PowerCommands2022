namespace PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

public class Cvssdata1
{
    public string version { get; set; }
    public string vectorString { get; set; }
    public string attackVector { get; set; }
    public string attackComplexity { get; set; }
    public string privilegesRequired { get; set; }
    public string userInteraction { get; set; }
    public string scope { get; set; }
    public string confidentialityImpact { get; set; }
    public string integrityImpact { get; set; }
    public string availabilityImpact { get; set; }
    public float baseScore { get; set; }
    public string baseSeverity { get; set; }
}