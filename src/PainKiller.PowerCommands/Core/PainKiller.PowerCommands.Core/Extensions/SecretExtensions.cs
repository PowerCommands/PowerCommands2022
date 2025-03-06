using PainKiller.PowerCommands.Security.Services;

namespace PainKiller.PowerCommands.Core.Extensions
{
    public static class SecretExtensions
    {
        public static string DecryptSecret(this SecretConfiguration secretConfiguration, string secretName)
        {
            var secret = secretConfiguration.Secrets.FirstOrDefault(s => s.Name == secretName);
            if (secret == null) return "";
            var retVal = SecretService.Service.GetSecret(secret.Name, secret.Options, EncryptionService.Service.DecryptString);
            return retVal;
        }
        public static string EncryptSecret<T>(this T configuration,  EnvironmentVariableTarget target, string secretName, string secret) where T : ICommandsConfiguration, new()
        {
            var existing = configuration.Secret.Secrets.FirstOrDefault(s => s.Name == secretName);
            if (existing != null) configuration.Secret.Secrets.Remove(existing);
            var secretConfiguration = new SecretItemConfiguration(target) { Name = secretName };
            configuration.Secret.Secrets.Add(secretConfiguration);
            var retVal = SecretService.Service.SetSecret(secretName, secret, secretConfiguration.Options,EncryptionService.Service.EncryptString);
            ConfigurationService.Service.SaveChanges(configuration);
            return retVal;
        }
    }
}