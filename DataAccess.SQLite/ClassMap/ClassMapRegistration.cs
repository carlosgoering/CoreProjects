using System.Reflection;
using DataAccess.Abstractions.Models;
using SQLite;

namespace DataAccess.SQLite.ClassMap;

internal static class ClassMapRegistration
{
    public static async Task Register<TEntity>(
        SQLiteAsyncConnection database)
        where TEntity : class, IBaseEntity, new()
    {
        var type = typeof(TEntity);

        var primaryKey = type
            .GetProperties()
            .SingleOrDefault(x =>
                x.GetCustomAttribute<PrimaryKeyAttribute>() != null);

        if (primaryKey is null)
            throw new InvalidOperationException(
                $"Entity '{type.Name}' must define a [PrimaryKey].");

        var columns = type
            .GetProperties()
            .Where(x => x.CanRead && x.CanWrite)
            .Select(x =>
            {
                var sqlType = GetSqlType(x.PropertyType);

                var definition = $"\"{x.Name}\" {sqlType}";

                if (x == primaryKey)
                    definition += " PRIMARY KEY";

                return definition;
            });

        var sql = $"""
            CREATE TABLE IF NOT EXISTS "{type.Name}"
            (
                {string.Join(",\n", columns)}
            );
            """;

        await database.ExecuteAsync(sql);
    }

    private static string GetSqlType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        return Type.GetTypeCode(type) switch
        {
            TypeCode.Int32 => "INTEGER",
            TypeCode.Int64 => "INTEGER",
            TypeCode.Int16 => "INTEGER",
            TypeCode.Byte => "INTEGER",
            TypeCode.Boolean => "INTEGER",
            TypeCode.Decimal => "REAL",
            TypeCode.Double => "REAL",
            TypeCode.Single => "REAL",
            TypeCode.DateTime => "TEXT",
            TypeCode.String => "TEXT",
            _ => "TEXT"
        };
    }
}