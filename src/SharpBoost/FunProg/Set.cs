using System;

namespace SharpBoost.FunProg {
    public delegate bool Set<in T>(T elem);

    public static class SetExtensions {
        public static Set<T> ToSingletonSet<T>(this T obj)
            where T : IEquatable<T> {
            return t => t.Equals(obj);
        }

        public static bool SetContains<T>(this Set<T> set, T elem) {
            return set(elem);
        }

        public static Set<T> SetUnion<T>(this Set<T> set, Set<T> other) {
            return x => set(x) || other(x);
        }

        public static Set<T> SetIntersect<T>(this Set<T> set, Set<T> other) {
            return x => set(x) && other(x);
        }

        public static Set<T> SetDiff<T>(this Set<T> set, Set<T> other) {
            return x => set(x) && !other(x);
        }

        public static Set<T> SetFilter<T>(this Set<T> set, Func<T, bool> filter ) {
            return x => set(x) && filter(x);
        }

    }
}
