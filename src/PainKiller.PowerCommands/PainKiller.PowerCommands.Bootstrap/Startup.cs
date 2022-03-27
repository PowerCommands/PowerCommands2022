using PainKiller.PowerCommands.Bootstrap.Configuration;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Security.DomainObjects;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Bootstrap
{
    public static class Startup
    {
        public static BaseCommandsConfiguration Initialize()
        {
            var configuration = ConfigurationManager.GetConfiguration<PowerCommandsConfiguration>().Configuration;
            var checkSum = new FileChecksum(configuration.KeyVault.Component);

            return configuration;
        }
    }
}