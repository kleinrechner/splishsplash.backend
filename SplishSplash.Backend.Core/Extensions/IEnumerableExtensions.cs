using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kleinrechner.SplishSplash.Backend.Core.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumarable)
        {
            return enumarable ?? Enumerable.Empty<T>();
        }

        public static List<T> EmptyIfNull<T>(this List<T> enumarable)
        {
            return enumarable ?? new List<T>();
        }

        public static IListComparisonResult<TOuter, TInner> Compare<TOuter, TInner, TKey>(this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector)
        {
            return Compare(outer, inner, outerKeySelector, innerKeySelector, null);
        }

        public static IListComparisonResult<TOuter, TInner> Compare<TOuter, TInner, TKey>(this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            IEqualityComparer<TKey> equalityComparer)
        {
            var result = new ListComparisonResult<TOuter, TInner>();
            var outerIdentifier = outer.EmptyIfNull().Select(outerKeySelector).ToList();
            var innerIdentifier = inner.EmptyIfNull().Select(innerKeySelector).ToList();

            result.Added = inner.Where(x => !outerIdentifier.Contains(innerKeySelector(x), equalityComparer)).ToArray();
            result.Updated = outer.Join(inner, outerKeySelector, innerKeySelector, (outerItem, innerItem) => new CudOperationUpdateItem<TOuter, TInner>(outerItem, innerItem)).ToArray();
            result.Removed = outer.Where(x => !innerIdentifier.Contains(outerKeySelector(x), equalityComparer)).ToArray();

            return result;
        }
    }
}
