namespace PainKiller.AzureKeyVault.Contracts
{
    public interface IKeyVaultConfig
    {
        string KeyVaultName { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string TenantId { get; }
        string KvUri { get; }
    }
}