namespace PainKiller.PowerCommands.Core.Managers
{
    public class ComponentManager<TConfiguration> where TConfiguration : CommandsConfiguration
    {
        private readonly TConfiguration _configuration;
        private readonly IDiagnosticManager _diagnostic;
        public ComponentManager(TConfiguration configuration, IDiagnosticManager diagnostic)
        {
            _configuration = configuration;
            _diagnostic = diagnostic;
        }
        public List<BaseComponentConfiguration> AutofixConfigurationComponents<T>(T configuration) where T : CommandsConfiguration, new()
        {
            var retVal = new List<BaseComponentConfiguration>();
            foreach (var component in configuration.Components)
            {
                component.Checksum = new FileChecksum(component.Component).Mde5Hash;
                retVal.Add(component);
            }
            return retVal;
        }
    }
}