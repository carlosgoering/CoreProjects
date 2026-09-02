using DataAccess.Abstractions.Interfaces;
using DataAccess.Abstractions.Models;
using Microsoft.Extensions.Options;
using SQLite;
using System.Linq.Expressions;

namespace DataAccess.SQLite.Context;

/// <summary>
/// If you want to know more about SQLite, please visit: https://github.com/praeclarum/sqlite-net
/// </summary>
/// <typeparam name="TEntity"></typeparam>
internal sealed class DataAccessContext<TEntity> : IDataAccessContext<TEntity> where TEntity : class, IBaseEntity, new()
{
    private readonly SQLiteAsyncConnection database;

    public DataAccessContext(IOptions<Database> databaseSettings)
    {
        var options = new SQLiteConnectionString(databaseSettings.Value.ConnectionString, true, databaseSettings.Value.ConnectionKey);
        database = new SQLiteAsyncConnection(options);

        database.CreateTableAsync<TEntity>();
    }

    public async Task InsertAsync(TEntity entity)
    {
        await database.InsertAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await database.UpdateAsync(entity);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await database.DeleteAsync(entity);
    }

    public async Task<TEntity?> SelectByIdAsync(string id)
    {
        return await database
            .Table<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Query<TEntity> query)
    {
        var table = ApplyFilters(
            database.Table<TEntity>(),
            query);

        table = ApplyOrder(table, query);

        return await table.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<TEntity>> SelectAsync(
        Query<TEntity> query)
    {
        var table = ApplyFilters(
            database.Table<TEntity>(),
            query);

        table = ApplyOrder(table, query);

        return await table
            .Skip(query.Skip)
            .Take(query.PageSize)
            .ToListAsync();
    }

    public async Task<PagedResult<TEntity>> SelectPagedAsync(
        Query<TEntity> query)
    {
        var table = ApplyFilters(
            database.Table<TEntity>(),
            query);

        var total = await table.CountAsync();

        table = ApplyOrder(table, query);

        var items = await table
            .Skip(query.Skip)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<TEntity>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }

    public async Task<long> CountAsync(
        Query<TEntity>? query = null)
    {
        var table = database.Table<TEntity>();

        if (query is not null)
            table = ApplyFilters(table, query);

        return await table.CountAsync();
    }

    public async Task<bool> ExistsAsync(Query<TEntity> query)
    {
        var table = ApplyFilters(
            database.Table<TEntity>(),
            query);

        return await table.CountAsync() > 0;
    }

    private static AsyncTableQuery<TEntity> ApplyFilters(
        AsyncTableQuery<TEntity> table,
        Query<TEntity> query)
    {
        foreach (var filter in query.Filters)
        {
            var expression = CreateExpression(filter);

            table = table.Where(expression);
        }

        return table;
    }

    private static AsyncTableQuery<TEntity> ApplyOrder(
        AsyncTableQuery<TEntity> table,
        Query<TEntity> query)
    {
        if (query.Order is null)
            return table;

        var property = typeof(TEntity)
            .GetProperty(query.Order.Field);

        if (property is null)
        {
            throw new InvalidOperationException(
                $"Property '{query.Order.Field}' was not found on '{typeof(TEntity).Name}'.");
        }

        var parameter = Expression.Parameter(
            typeof(TEntity),
            "x");

        var member = Expression.Property(
            parameter,
            property);

        var lambda = Expression.Lambda(
            typeof(Func<,>).MakeGenericType(
                typeof(TEntity),
                property.PropertyType),
            member,
            parameter);

        var method = query.Order.Descending
            ? "OrderByDescending"
            : "OrderBy";

        var orderMethod = typeof(Queryable)
            .GetMethods()
            .Single(x =>
                x.Name == method &&
                x.GetParameters().Length == 2);

        var genericMethod = orderMethod.MakeGenericMethod(
            typeof(TEntity),
            property.PropertyType);

        return (AsyncTableQuery<TEntity>)
            genericMethod.Invoke(
                null,
                [table, lambda])!;
    }

    private static Expression<Func<TEntity, bool>> CreateExpression(
        QueryFilter filter)
    {
        var parameter = Expression.Parameter(
            typeof(TEntity),
            "x");

        var property = typeof(TEntity)
            .GetProperty(filter.Field);

        if (property is null)
        {
            throw new InvalidOperationException(
                $"Property '{filter.Field}' was not found on '{typeof(TEntity).Name}'.");
        }

        var member = Expression.Property(
            parameter,
            property);

        var value = Expression.Constant(
            ConvertValue(filter.Value, property.PropertyType),
            property.PropertyType);

        Expression body = filter.Operator switch
        {
            QueryOperator.Equal =>
                Expression.Equal(member, value),

            QueryOperator.NotEqual =>
                Expression.NotEqual(member, value),

            QueryOperator.GreaterThan =>
                Expression.GreaterThan(member, value),

            QueryOperator.GreaterThanOrEqual =>
                Expression.GreaterThanOrEqual(member, value),

            QueryOperator.LessThan =>
                Expression.LessThan(member, value),

            QueryOperator.LessThanOrEqual =>
                Expression.LessThanOrEqual(member, value),

            _ => throw new NotSupportedException(
                $"Operator '{filter.Operator}' is not supported by SQLite.")
        };

        return Expression.Lambda<Func<TEntity, bool>>(
            body,
            parameter);
    }

    private static object? ConvertValue(
        object? value,
        Type targetType)
    {
        if (value is null)
            return null;

        var underlyingType =
            Nullable.GetUnderlyingType(targetType)
            ?? targetType;

        if (underlyingType.IsInstanceOfType(value))
            return value;

        if (underlyingType.IsEnum)
            return Enum.Parse(
                underlyingType,
                value.ToString()!,
                true);

        return Convert.ChangeType(
            value,
            underlyingType);
    }
}
