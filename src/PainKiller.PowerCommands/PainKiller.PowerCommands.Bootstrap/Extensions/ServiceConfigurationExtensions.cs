using PainKiller.PowerCommands.MyExampleCommands;
using PainKiller.PowerCommands.Security.DomainObjects;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Bootstrap.Extensions;

public static class ServiceConfigurationExtensions
{
    public static PowerCommandServices ShowDiagnostic(this PowerCommandServices services, bool showDiagnostic)
    {
        services.Configuration.ShowDiagnosticInformation = showDiagnostic;
        return services;
    }
    public static PowerCommandServices AddComponent(this PowerCommandServices services, string name, string assemblyName)
    {
        var fileCheckSum = new FileChecksum(assemblyName).Mde5Hash;
        services.Configuration.Components.Add(new BaseComponentConfiguration{Checksum = fileCheckSum,Component = assemblyName,Name = name});
        return services;
    }
}