using System.Linq.Expressions;

namespace RepoRanger.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IOrderedQueryable<T> OrderByColumn<T>(this IQueryable source, string columnPath) 
        => source.OrderByColumnUsing<T>(columnPath, "OrderBy");

    public static IOrderedQueryable<T> OrderByColumnDescending<T>(this IQueryable source, string columnPath) 
        => source.OrderByColumnUsing<T>(columnPath, "OrderByDescending");
    
    private static IOrderedQueryable<T> OrderByColumnUsing<T>(this IQueryable source, string columnPath, string method)
    {
        var parameter = Expression.Parameter(typeof(T), "item");
        var member = columnPath.Split('.')
            .Aggregate((Expression)parameter, Expression.PropertyOrField);
        
        var keySelector = Expression.Lambda(member, parameter);
        var methodCall = Expression.Call(typeof(Queryable), method, [parameter.Type, member.Type],
            source.Expression, Expression.Quote(keySelector));

        return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
    }
}