using System.Reflection;

namespace Dataaccess.Abstractions.Attributes;

public sealed class EntityMapping
{
    public IReadOnlyList<PropertyInfo> PrimaryKeys { get; init; } = [];
    public IReadOnlyList<PropertyInfo> Indexed { get; init; } = [];
    public IReadOnlyList<PropertyInfo> Unique { get; init; } = [];
}