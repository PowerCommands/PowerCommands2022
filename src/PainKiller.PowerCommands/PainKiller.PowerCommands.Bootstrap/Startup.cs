using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.DomainObjects;

namespace PainKiller.PowerCommands.Bootstrap
{
    public static class Startup
    {
        public static PowerCommandsConfiguration Initialize()
        {
            var configuration = ConfigurationManager.GetConfiguration<PowerCommandsConfiguration>();
            return configuration.Configuration;
        }
    }
}