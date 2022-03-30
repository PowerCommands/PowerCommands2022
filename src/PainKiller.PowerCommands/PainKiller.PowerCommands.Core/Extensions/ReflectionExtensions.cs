﻿using System.ComponentModel;
using System.Reflection;

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
}