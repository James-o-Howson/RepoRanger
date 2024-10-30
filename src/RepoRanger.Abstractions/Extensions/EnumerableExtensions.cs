namespace RepoRanger.Abstractions.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<TItem>> Batch<TItem>(this IEnumerable<TItem> source, int size)
    {
        TItem[]? bucket = null;
        var count = 0;

        foreach (var item in source)
        {
            bucket ??= new TItem[size];

            bucket[count++] = item;
            if (count != size) continue;

            yield return bucket;

            bucket = null;
            count = 0;
        }

        if (bucket != null && count > 0) yield return bucket.Take(count);
    }
}