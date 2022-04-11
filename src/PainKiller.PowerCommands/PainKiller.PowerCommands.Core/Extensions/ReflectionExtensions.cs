using System.ComponentModel;
using System.Reflection;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Extensions;

public static class ReflectionExtensions
{
    public static string GetDescription(this Type type)
    {
        var attributes = type.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
        return attributes.Length == 0 ? "" : ((DescriptionAttribute) attributes.First()).Description;
    }

    /// <summary>
    /// Get properties of T
    /// </summary>
    /// <typeparam name="T">The type to look for</typeparam>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    public static List<PropertyInfo> GetPropertiesOfT<T>(this Type instanceType) where T : new()
    {
        var propertyInfos = instanceType.GetProperties().Where(t => t.PropertyType.BaseType == typeof(T)).ToList();
        return propertyInfos;
    }

    public static PowerCommandAttribute GetPowerCommandAttribute(this IConsoleCommand command)
    {
        if (command == null) return new PowerCommandAttribute(description: "No command found...");
        var attributes = command.GetType().GetCustomAttributes(typeof(PowerCommandAttribute), inherit: false);
        return attributes.Length == 0 ? new PowerCommandAttribute(description:"Command have no description attribute") : (PowerCommandAttribute)attributes.First();
    }

    public static string GetDefaultParameter(this IConsoleCommand command)
    {
        if (command == null) return "";
        var attribute = command.GetType().GetCustomAttributes(typeof(PowerCommandAttribute), inherit: false).FirstOrDefault() as PowerCommandAttribute;
        return attribute is null ? "" : attribute.DefaultParameter;
    }
}