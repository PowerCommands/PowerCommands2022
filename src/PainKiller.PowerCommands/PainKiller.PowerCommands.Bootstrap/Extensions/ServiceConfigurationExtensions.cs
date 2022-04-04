using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.MyExampleCommands;
using PainKiller.PowerCommands.Security.DomainObjects;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Bootstrap.Extensions;

public static class ServiceConfigurationExtensions
{
    public static PowerCommandServices ShowDiagnostic(this PowerCommandServices services, bool showDiagnostic)
    {
        services.Configuration.ShowDiagnosticInformation = showDiagnostic;
        services.Diagnostic.ShowDiagnostic = showDiagnostic;
        return services;
    }
    public static PowerCommandServices SetMetadata(this PowerCommandServices services, Metadata metadata)
    {
        services.Configuration.Metadata = metadata;
        return services;
    }
    /// <summary>Change must be persisted, restart needed</summary>
    public static PowerCommandServices SetLogMinimumLevel(this PowerCommandServices services, LogLevel logLevel)
    {
        services.Configuration.Log.RestrictedToMinimumLevel = logLevel.ToString();
        return services;
    }
    public static PowerCommandServices AddComponent(this PowerCommandServices services, string name, string assemblyName)
    {
        var fileCheckSum = new FileChecksum(assemblyName).Mde5Hash;
        services.Configuration.Components.Add(new BaseComponentConfiguration{Checksum = fileCheckSum,Component = assemblyName,Name = name});
        return services;
    }
    public static PowerCommandServices PersistChanges(this PowerCommandServices services)
    {
        ConfigurationManager.SaveChanges(services.Configuration, "");
        return services;
    }
}