using System;
using System.Collections.Generic;

namespace SharpBoost {
    public static class DictionaryExtensions {
        public static OrdDictionary<TKey, TValue, TEqualityComparer> ToOrdDictionary<TKey, TValue, TEqualityComparer>(
            this IDictionary<TKey, TValue> dictionary, TEqualityComparer comparer)
        where TEqualityComparer : IEqualityComparer<TKey> {
            return new OrdDictionary<TKey, TValue, TEqualityComparer>(dictionary.ArgumentNullCheck("dictionary"), comparer);
        }

        public static OrdDictionary<TKey, TValue, IEqualityComparer<TKey>> ToOrdDictionary<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            Func<TKey, TKey, bool> equals, Func<TKey, int> hash = null) {
            return dictionary.ToOrdDictionary(ComparerFactory.CreateEqualityComparer(equals, hash));
        }


        public static OrdDictionary<string, TValue, IEqualityComparer<string>> ToCaseInsensitiveKeysDictionary<TValue>(
            this IDictionary<string, TValue> dictionary) {
            return dictionary.ToOrdDictionary((IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase);
        }

    }

    public sealed class OrdDictionary<TKey, TValue, TEqualityComparer> : Dictionary<TKey, TValue> where TEqualityComparer : IEqualityComparer<TKey> {
        public OrdDictionary(TEqualityComparer comparer) : base(comparer) { }
        public OrdDictionary(int capacity, TEqualityComparer comparer) : base(capacity, comparer) { }
        public OrdDictionary(IDictionary<TKey, TValue> dictionary, TEqualityComparer comparer) : base(dictionary, comparer) { }
    }
}
