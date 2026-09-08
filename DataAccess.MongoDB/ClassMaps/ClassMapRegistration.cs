using System.Reflection;
using DataAccess.Abstractions.Attributes;
using DataAccess.Abstractions.Models;
using MongoDB.Bson.Serialization;

namespace DataAccess.MongoDB.ClassMaps
{
internal static class ClassMapRegistration
{
    private static readonly HashSet<Type> registeredTypes = [];

    public static void Register<TEntity>()
        where TEntity : class, IBaseEntity, new()
    {
        var type = typeof(TEntity);

        if (registeredTypes.Contains(type))
            return;

        var primaryKey = type
            .GetProperties()
            .SingleOrDefault(x =>
                x.GetCustomAttribute<PrimaryKeyAttribute>() != null);

        if (primaryKey == null)
            throw new InvalidOperationException(
                $"Entity '{type.Name}' must have a [PrimaryKey].");

        BsonClassMap.RegisterClassMap<TEntity>(cm =>
        {
            cm.AutoMap();

            var member = cm.GetMemberMap(primaryKey.Name);

            cm.SetIdMember(member);
        });

        registeredTypes.Add(type);
    }
}
}
