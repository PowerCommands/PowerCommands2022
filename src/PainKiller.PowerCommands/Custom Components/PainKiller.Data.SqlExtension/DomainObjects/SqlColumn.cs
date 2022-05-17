using System.Reflection;
using PainKiller.Data.SqlExtension.Extensions;

namespace PainKiller.Data.SqlExtension.DomainObjects
{
    
    public class SqlColumn
    {
        public SqlColumn(string name)
        {
            Name = $"[{name}]";
        }
        public SqlColumn(PropertyInfo propertyInfo, object instance)
        {
            Name = $"[{propertyInfo.Name}]";
            Value = propertyInfo.GetValue(instance).ToSqlFormattedValue();
        }
        public bool IsPrimaryKey { get; set; }
        public string Name { get; }
        /// <summary>
        /// Name.toLower() with [] escaped
        /// </summary>
        public string CompareName => Name.ToLower().Replace("[", "").Replace("]", "");
        public string Value { get; }

    }
}