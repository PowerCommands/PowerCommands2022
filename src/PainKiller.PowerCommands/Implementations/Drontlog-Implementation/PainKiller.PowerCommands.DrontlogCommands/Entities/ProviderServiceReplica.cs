using PainKiller.Data.SqlExtension.DomainObjects;

namespace PainKiller.PowerCommands.DrontlogCommands.Entities;

[TableMetadata(nameof(ProviderServiceReplica), nameof(ProviderServiceID))]
public class ProviderServiceReplica
{
    public Guid ProviderServiceID { get; set; }
    public string UrlToLogo { get; set; } = "";
}