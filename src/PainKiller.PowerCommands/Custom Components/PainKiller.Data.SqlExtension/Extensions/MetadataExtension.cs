using System;
using System.Linq;
using System.Reflection;
using PainKiller.Data.SqlExtension.DomainObjects;

namespace PainKiller.Data.SqlExtension.Extensions
{
    public static class MetadataExtension
    {
        public static TableMetadata GeTableMetadata<T>(this T entity)
        {
            var type = typeof(T);
            var columns = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => new SqlColumn(p, entity)).ToArray();
            
            var attributes = typeof(T).GetCustomAttributes();
            var attribute = (TableMetadataAttribute)attributes.FirstOrDefault();
            if (attribute != null)
            {
                var pkColumn = columns.FirstOrDefault(c => c.CompareName == attribute.TableInfo.PrimaryKeys.ToLower());
                if (pkColumn != null) pkColumn.IsPrimaryKey = true;
                return new TableMetadata(attribute.TableInfo.TableName, attribute.TableInfo.PrimaryKeys, columns);
            }
            
            var tableName = type.Name;
            var primaryKeyName = "id";
            //First attempt does the table have column only named id? In that case it is probably the Primary key column
            var pkCol = columns.FirstOrDefault(c => c.CompareName == primaryKeyName);
            
            if ( pkCol != null)
            {
                pkCol.IsPrimaryKey = true;
                return new TableMetadata(tableName, primaryKeyName, columns);
            }
            primaryKeyName = $"{tableName}{primaryKeyName}".ToLower();
            //Second attempt using tablename + id naming convention
            pkCol = columns.FirstOrDefault(c => c.CompareName == primaryKeyName);
            if (pkCol != null) 
            {
                pkCol.IsPrimaryKey = true;
                return new TableMetadata(tableName, $"{tableName}ID", columns);
            }
            //Third option is to return metadata with a empty primaryKey, update and delete will not be possible
            return new TableMetadata(tableName,primaryKeys:"", columns);
        }
        public static bool IsNumericDatatype(this TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}