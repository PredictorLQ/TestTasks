using System.Diagnostics.CodeAnalysis;
using Айдиджит_Групп.Assistant;

namespace Айдиджит_Групп.Models;

public static class PageExpansion
{
    public static IEnumerable<TEntity> GetPageItems<TEntity>([NotNull] this IEnumerable<TEntity> items, uint? skip = null, uint? take = null)
    {
        if (skip.HasValue && skip.Value > 0)
            items = items.Skip((int)skip);

        if (take.HasValue && take.Value > 0)
            items = items.Take((int)take);

        return items;
    }

    public static Page<TEntity> ToPage<TEntity>([NotNull] this IEnumerable<TEntity> items, uint? skip = null, uint? take = null) where TEntity : class
    {
        var count = items.Count();

        if (count == 0)
            return new Page<TEntity>();

        var itemsPage = items
            .GetPageItems(skip, take)
            .ToArray();

        var takeMax = (int?)take ?? itemsPage.Length;

        var countPages = count / takeMax;

        if (count % takeMax > 0)
            countPages++;

        var nowPage = (int)(skip ?? 0) / takeMax;
        nowPage++;

        return new(itemsPage, countPages, nowPage, count);
    }
}