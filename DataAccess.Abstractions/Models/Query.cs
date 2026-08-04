using System.Linq.Expressions;

namespace DataAccess.Abstractions.Models;

public sealed record Query<TEntity>
{
    public Expression<Func<TEntity, bool>>? Filter { get; init; }

    public Expression<Func<TEntity, object>>? OrderBy { get; init; }

    public bool Descending { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 20;

    public string? Search { get; init; }

    public int Skip => (Page - 1) * PageSize;

    public int Take => PageSize;
}