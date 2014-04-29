using System;

namespace SharpBoost.FunProg {
    public static class Lambda {
        public static Func<Unit> A(Action a) {
            return a.ToFunc();
        }

        public static Func<T, Unit> A<T>(Action<T> a) {
            return a.ToFunc();
        }

        public static Func<T, T1, Unit> A<T, T1>(Action<T, T1> a) {
            return a.ToFunc();
        }

        public static Func<T, T1, T2, Unit> A<T, T1, T2>(Action<T, T1, T2> a) {
            return a.ToFunc();
        }

        public static Func<T, T1, T2, T3, Unit> A<T, T1, T2, T3>(Action<T, T1, T2, T3> a) {
            return a.ToFunc();
        }

        public static Func<T, T1, T2, T3, T4, Unit> A<T, T1, T2, T3, T4>(Action<T, T1, T2, T3, T4> a) {
            return a.ToFunc();
        }

        public static Func<T> F<T>(Func<T> f) {
            return f;
        }

        public static Func<T, T1> F<T, T1>(Func<T, T1> f) {
            return f;
        }

        public static Func<T, T1, T2> F<T, T1, T2>(Func<T, T1, T2> f) {
            return f;
        }

        public static Func<T, T1, T2, T3> F<T, T1, T2, T3>(Func<T, T1, T2, T3> f) {
            return f;
        }

        public static Func<T, T1, T2, T3, T4> F<T, T1, T2, T3, T4>(Func<T, T1, T2, T3, T4> f) {
            return f;
        }

    }
}
