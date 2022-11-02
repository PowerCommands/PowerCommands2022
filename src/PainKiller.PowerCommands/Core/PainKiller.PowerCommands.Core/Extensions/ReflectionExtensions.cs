using System.ComponentModel;
using System.Reflection;
using System.Text.Json;

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

    public static void SetPropertyValue<T>(this T instance, string propertyName, string propertyValue) where T : new()
    {
        if (instance == null) return;
        var propertyInfo = instance.GetType().GetProperties().FirstOrDefault(t => t.Name == propertyName);
        if (propertyInfo == null) return;
        propertyInfo.SetValue(instance, propertyValue);
    }

    public static object GetPropertyValue<T>(this T instance, string propertyName) where T : new()
    {
        if (instance == null) return new object() ;
        var propertyInfo = instance.GetType().GetProperties().FirstOrDefault(t => t.Name == propertyName);
        return propertyInfo == null ? new object() : propertyInfo.GetValue(instance)!;
    }

    public static PowerCommandAttribute GetPowerCommandAttribute(this IConsoleCommand command)
    {
        var attributes = command.GetType().GetCustomAttributes(typeof(PowerCommandAttribute), inherit: false);
        return attributes.Length == 0 ? new PowerCommandAttribute(description:"Command have no description attribute") : (PowerCommandAttribute)attributes.First();
    }
    public static string GetDefaultParameter(this IConsoleCommand command)
    {
        var attribute = command.GetType().GetCustomAttributes(typeof(PowerCommandAttribute), inherit: false).FirstOrDefault() as PowerCommandAttribute;
        return attribute is null ? "" : attribute.Suggestion;
    }
    public static T DeepClone<T>(this T objSource) where T : class => CopyObject<T>(objSource);

    public static T CopyObject<T>(object objSource)
    {
        using MemoryStream stream = new MemoryStream();
        string jsonString = JsonSerializer.Serialize(objSource);
        return JsonSerializer.Deserialize<T>(jsonString)!;
    }

}