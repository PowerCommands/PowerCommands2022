namespace PainKiller.AzureKeyVault.Contracts
{
    public interface IKeyVaultService
    {
        bool SetSecret(string secretName, string secretValue);
        string GetSecret(string secretName);
        IEnumerable<string> GetSecrets();
    }
}