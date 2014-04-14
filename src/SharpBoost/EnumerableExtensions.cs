using System;
using System.Collections.Generic;

namespace SharpBoost {
    public static class EnumerableExtensions {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
            // ReSharper disable PossibleMultipleEnumeration
            items.ArgumentNullCheck("items");
            action.ArgumentNullCheck("action");

            foreach (var item in items)
                action(item);
            // ReSharper restore PossibleMultipleEnumeration
        }


    }
}
