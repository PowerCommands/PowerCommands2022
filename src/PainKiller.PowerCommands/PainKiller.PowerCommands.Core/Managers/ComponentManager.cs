using System.Text;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Security.DomainObjects;
using PainKiller.PowerCommands.Security.Extensions;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Core.Managers;

public class ComponentManager<TConfiguration> where TConfiguration : CommandsConfiguration
{
    private readonly TConfiguration _configuration;
    private readonly IDiagnosticManager _diagnostic;

    public ComponentManager(TConfiguration configuration, IDiagnosticManager diagnostic)
    {
        _configuration = configuration;
        _diagnostic = diagnostic;
    }
    public bool ValidateConfigurationWithComponents()
    {
        var retVal = true;
        var components = typeof(TConfiguration).GetPropertiesOfT<BaseComponentConfiguration>().Select(c => c.GetValue(_configuration) as BaseComponentConfiguration).ToList();
        if(_configuration.Components.Count > 0) components.AddRange(_configuration.Components);
        
        foreach (var component in components)
        {
            if(component is null) continue;
            var fileCheckSum = new FileChecksum(component.Component);
            var validateCheckSum = fileCheckSum.CompareFileChecksum(component.Checksum);
            if(!validateCheckSum) retVal = false;
            _diagnostic.Message($"{component.Name} Checksum {fileCheckSum.Mde5Hash} {validateCheckSum}");
        }
        return retVal;
    }
    
    //TODO: Create a util application that uses this
    public string AutofixConfigurationComponents<T>(T configuration) where T : CommandsConfiguration, new()
    {
        var retVal = new StringBuilder();
        foreach (var component in configuration.Components)
        {
            var fileCheckSum = new FileChecksum(component.Component);
            var validateCheckSum = fileCheckSum.CompareFileChecksum(component.Checksum);
            _diagnostic.Message($"{component.Name} Checksum {fileCheckSum.Mde5Hash} {validateCheckSum}");
            if (!validateCheckSum)
            {
                component.Checksum = fileCheckSum.Mde5Hash;
                ConfigurationManager.SaveChanges(configuration);
                retVal.AppendLine($"{component.Name} Checksum {fileCheckSum.Mde5Hash} {validateCheckSum} FIXED");
            }
        }
        return retVal.ToString();
    }

}