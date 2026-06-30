using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Metabase.Extensions;

public enum OrderDirection
{
    ASCENDING,
    DESCENDING
}

public static class LinqExtensions
{
    [Pure]
    public static IEnumerable<T> If<T>(
        this IEnumerable<T> source,
        bool condition,
        Func<IEnumerable<T>, IEnumerable<T>> transform
    )
    {
        return condition ? transform(source) : source;
    }

    [Pure]
    public static IQueryable<T> If<T>(
        this IQueryable<T> source,
        bool condition,
        Func<IQueryable<T>,
        IQueryable<T>> transform
    )
    {
        return condition ? transform(source) : source;
    }

    [Pure]
    public static List<T> IfList<T>(
        this List<T> source,
        bool condition,
        Func<List<T>, List<T>> transform
    )
    {
        return condition ? transform(source) : source;
    }

    [Pure]
    public static List<T> ToReversed<T>(this List<T> source)
    {
        var copy = new List<T>(source);
        copy.Reverse();
        return copy;
    }

    [Pure]
    public static T? GetAtOrDefault<T>(this T[] array, int index, T? defaultValue = default) where T : class
    {
        return (index >= 0 && index < array.Length) ? array[index] : defaultValue;
    }

    [Pure]
    public static T? GetFirstOrDefault<T>(this T[] array) where T : class
    {
        return array.Length > 0 ? array[0] : default;
    }

    [Pure]
    public static T? GetAtOrDefault<T>(this IReadOnlyList<T> list, int index) where T : class
    {
        return (index >= 0 && index < list.Count) ? list[index] : default;
    }

    [Pure]
    public static T? GetFirstOrDefault<T>(this IReadOnlyList<T> list) where T : class
    {
        return list.Count > 0 ? list[0] : default;
    }

    [Pure]
    public static T? GetLastOrDefault<T>(this IReadOnlyList<T> list) where T : class
    {
        return list.Count > 0 ? list[^1] : default;
    }

    [Pure]
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : class
    {
        return enumerable.Where(item => item is not null).Select(item => item!);
    }

    [Pure]
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : struct
    {
        return enumerable.Where(item => item.HasValue).Select(item => item!.Value);
    }

    [Pure]
    public static IOrderedQueryable<T> OrderByDirection<T, TKey>(
        this IQueryable<T> source,
        Expression<Func<T, TKey>> keySelector,
        OrderDirection direction
    )
    {
        return direction is OrderDirection.ASCENDING
            ? source.OrderBy(keySelector)
            : source.OrderByDescending(keySelector);
    }


    [Pure]
    public static IEnumerable<T> Interleave<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        var enumerators = new LinkedList<IEnumerator<T>>();
        try
        {
            foreach (var sequence in sequences)
            {
                var enumerator = sequence.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    enumerators.AddLast(enumerator);
                    yield return enumerator.Current;
                }
                else
                {
                    enumerator.Dispose();
                }
            }
            var node = enumerators.First;
            while (node is { Value: var enumerator, Next: var nextNode })
            {
                if (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
                else
                {
                    enumerators.Remove(node);
                    enumerator.Dispose();
                }
                node = nextNode ?? enumerators.First;
            }
        }
        finally
        {
            foreach (var enumerator in enumerators)
                enumerator.Dispose();
        }
    }

    [Pure]
    public static IEnumerable<TResult> Scan<TSource, TResult, TAccumulate>(
        this IEnumerable<TSource> source,
        TAccumulate seed,
        Func<TAccumulate, TSource, (TAccumulate, TResult)> function)
    {
        var accumulate = seed;
        foreach (var item in source)
        {
            (accumulate, var result) = function(accumulate, item);
            yield return result;
        }
    }

    [Pure]
    public static List<T> Rotate<T>(
        this List<T> list,
        Predicate<T> after
    )
        where T : class
    {
        if (list.Count is 0)
        {
            return list;
        }
        var afterIndex = list.FindIndex(after);
        if (afterIndex is -1)
        {
            return list;
        }
        var index = (afterIndex + 1) % list.Count;
        var result = new List<T>(list.Count);
        var span = CollectionsMarshal.AsSpan(list);
        result.AddRange(span.Slice(index));
        result.AddRange(span.Slice(0, index));
        return result;
    }
}