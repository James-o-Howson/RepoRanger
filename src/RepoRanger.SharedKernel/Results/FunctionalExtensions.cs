namespace RepoRanger.SharedKernel.Results;

public static class FunctionalExtensions
{
    public static async Task<TOut> Map<TIn, TOut>(this Task<TIn> task, Func<TIn, TOut> func) => func(await task);
}