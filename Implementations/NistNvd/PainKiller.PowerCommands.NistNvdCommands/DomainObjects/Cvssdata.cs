namespace PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

public class Cvssdata
{
    public string version { get; set; }
    public string vectorString { get; set; }
    public string accessVector { get; set; }
    public string accessComplexity { get; set; }
    public string authentication { get; set; }
    public string confidentialityImpact { get; set; }
    public string integrityImpact { get; set; }
    public string availabilityImpact { get; set; }
    public float baseScore { get; set; }
    public string baseSeverity { get; set; }
}