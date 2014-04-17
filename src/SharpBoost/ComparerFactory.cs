using System;
using System.Collections.Generic;

namespace SharpBoost {
    public static class ComparerFactory {
        public static IEqualityComparer<T> CreateEqualityComparer<T>(Func<T, T, bool> equals, Func<T, int> hash = null) {
            return new EqualityComparer<T>(equals, hash);
        }

        public static IComparer<T> CreateComparer<T>(Func<T, T, int> compare) {
            return new Comparer<T>(compare);
        }
    }

    class Comparer<T> : IComparer<T> {
        private readonly Func<T, T, int> _compare;
        public Comparer(Func<T, T, int> compare) {
            _compare = compare.ArgumentNullCheck("_compare");
        }


        #region IComparer<T> Members

        int IComparer<T>.Compare(T x, T y) {
            return _compare(x, y);
        }

        #endregion
    }

    class EqualityComparer<T> : IEqualityComparer<T> {
        private readonly Func<T, T, bool> _equals;
        private readonly Func<T, int> _hash;

        public EqualityComparer(Func<T, T, bool> equals, Func<T, int> hash = null) {
            _equals = equals.ArgumentNullCheck("equals");
            _hash = hash;
        }

        #region IEqualityComparer<T> Members

        bool IEqualityComparer<T>.Equals(T x, T y) {
            return _equals(x, y);
        }

        int IEqualityComparer<T>.GetHashCode(T obj) {
            return _hash.Return(f => f(obj), obj.GetHashCode());
        }

        #endregion

    }

}
