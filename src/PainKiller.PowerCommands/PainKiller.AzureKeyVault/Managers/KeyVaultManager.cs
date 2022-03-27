using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using PainKiller.AzureKeyVault.Contracts;

namespace PainKiller.AzureKeyVault.Managers
{
    //https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis#add-credentials-to-your-web-application
    public class KeyVaultManager : IKeyVaultManager
    {
        private readonly SecretClient _client;
        public KeyVaultManager(IKeyVaultConfig config)
        {
            _client = new SecretClient(new Uri(config.KvUri), new ClientSecretCredential(config.TenantId, config.ClientId, config.ClientSecret));
        }
        public bool SetSecret(string secretName, string secretValue)
        {
            try
            {
                _client.SetSecret(secretName, secretValue);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return true;
        }
        public string GetSecret(string secretName)
        {
            try
            {
                var retVal = _client.GetSecret(secretName);
                return retVal.Value.Value;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }
        public IEnumerable<string> GetSecrets()
        {
            var secrets = _client.GetPropertiesOfSecrets(new CancellationToken(false)).Select(s => s.Name).OrderBy(s => s).ToArray();
            return secrets;
        }
    }
}
