namespace PainKiller.PowerCommands.DrontlogCommands.Entities;

public class ProviderService
{
    public Guid ProviderServiceID { get; set; }
    public Guid IntegrationID { get; set; }
    public string Name { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Filter { get; set; } = "";
    public string Url { get; set; } = "";
    public string UrlToLogo { get; set; } = "";
    public short PostCount { get; set; }
    public bool NoCache { get; set; }
    public string TemplateName { get; set; } = "";
    public bool Disabled { get; set; }
    public bool AutoUpdate { get; set; }
    public string DefaultTag { get; set; } = "";
    public bool Searchable { get; set; }
    public bool Archive { get; set; }
}