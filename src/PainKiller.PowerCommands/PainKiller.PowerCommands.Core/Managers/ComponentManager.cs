using System.Text;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Security.DomainObjects;
using PainKiller.PowerCommands.Security.Extensions;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Core.Managers;

public class ComponentManager<TConfiguration> where TConfiguration : BasicCommandsConfiguration
{
    private readonly TConfiguration _configuration;

    public ComponentManager(TConfiguration configuration)
    {
        _configuration = configuration;
    }
    public bool ValidateConfigurationWithComponents()
    {
        var retVal = true;
        var components = typeof(TConfiguration).GetPropertiesOfT<BaseComponentConfiguration>();
        
        foreach (var propertyInfo in components)
        {
            if (propertyInfo.GetValue(_configuration) is not BaseComponentConfiguration instance) continue;
            var fileCheckSum = new FileChecksum(instance.Component);
            var validateCheckSum = fileCheckSum.CompareFileChecksum(instance.Checksum);
            if(!validateCheckSum) retVal = false;
            if(_configuration.ShowDiagnosticInformation) Console.WriteLine($"{instance.Name ?? "null"} Checksum {fileCheckSum.Mde5Hash} {validateCheckSum}");
        }
        return retVal;
    }
    public string AutofixConfigurationComponents<T>(T configuration) where T : BasicCommandsConfiguration, new()
    {
        var retVal = new StringBuilder();
        foreach (var component in configuration.Components)
        {
            var fileCheckSum = new FileChecksum(component.Component);
            var validateCheckSum = fileCheckSum.CompareFileChecksum(component.Checksum);
            if (_configuration.ShowDiagnosticInformation) Console.WriteLine($"{component.Name ?? "null"} Checksum {fileCheckSum.Mde5Hash} {validateCheckSum}");
            if (!validateCheckSum)
            {
                component.Checksum = fileCheckSum.Mde5Hash;
                ConfigurationManager.Update(configuration);
                retVal.AppendLine($"{component.Name ?? "null"} Checksum {fileCheckSum.Mde5Hash} {validateCheckSum} FIXED");
            }
        }
        return retVal.ToString();
    }

}