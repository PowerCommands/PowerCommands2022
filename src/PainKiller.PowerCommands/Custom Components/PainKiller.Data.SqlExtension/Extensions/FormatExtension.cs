using System;

namespace PainKiller.Data.SqlExtension.Extensions
{
    public static class FormatExtension
    {
        public static string ToSqlFormattedValue<T>(this T value)
        {
            if (value == null) return "null";

            var typeCode = Type.GetTypeCode(value.GetType());
            if (typeCode == TypeCode.Boolean)
            {
                var boolValue = Convert.ToBoolean(value);
                return boolValue ? "1" : "0";
            }
            var retVal = typeCode.IsNumericDatatype() ? $"{value}" : $"'{value.ToString().Replace("'","\"")}'";
            return retVal;
        }
    }
}