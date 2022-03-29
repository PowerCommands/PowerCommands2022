using System.Text;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Security.DomainObjects;
using PainKiller.PowerCommands.Security.Extensions;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Core.Managers;

public class PluginManager<TConfiguration> where TConfiguration : BasicCommandsConfiguration
{
    private readonly TConfiguration _configuration;

    public PluginManager(TConfiguration configuration)
    {
        _configuration = configuration;
    }
    //public List<IConsoleCommand> GetCommands()
    //{
    //    var retVal = new List<IConsoleCommand>();
    //    //Run through all plugin dll files that are included in the configuration
    //    foreach (var pluginInfo in _configuration.PowerCommandPluginInfos.Where(p => !p.Disabled))
    //    {
    //        var commands = ReflectionManager.GetCommands(pluginInfo);
    //        retVal.AddRange(commands);
    //    }
    //    return retVal;
    //}

    public bool ValidateConfigurationWithPlugins()
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

    protected string AutofixConfigurationPlugins<T>(T configuration) where T : BasicCommandsConfiguration, new()
    {
        var retVal = new StringBuilder();
        var components = typeof(TConfiguration).GetPropertiesOfT<BaseComponentConfiguration>();

        foreach (var propertyInfo in components)
        {
            if (propertyInfo.GetValue(_configuration) is not BaseComponentConfiguration instance) continue;
            var fileCheckSum = new FileChecksum(instance.Component);
            var validateCheckSum = fileCheckSum.CompareFileChecksum(instance.Checksum);
            if (_configuration.ShowDiagnosticInformation) Console.WriteLine($"{instance.Name ?? "null"} Checksum {fileCheckSum.Mde5Hash} {validateCheckSum}");
            if (!validateCheckSum)
            {
                propertyInfo.SetValue(instance, fileCheckSum.Mde5Hash);
                ConfigurationManager.Update(configuration);
                retVal.AppendLine($"{instance.Name ?? "null"} Checksum {fileCheckSum.Mde5Hash} {validateCheckSum} FIXED");
            }
        }
        return retVal.ToString();
    }

}