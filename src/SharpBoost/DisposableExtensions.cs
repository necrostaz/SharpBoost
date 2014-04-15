using System;

namespace SharpBoost {
    public static class DisposableExtensions {
        public static void Use<T>(this T resource, Action<T> action) where T : IDisposable {
            using (resource) {
                action(resource);
            }
        }

        public static void Use<T1, T2>(this T1 resource, T2 resource2, Action<T1, T2> action)
            where T1 : IDisposable
            where T2 : IDisposable {
            using (resource)
            using (resource2) {
                action(resource, resource2);
            }
        }

        public static void Use<T1, T2, T3>(this T1 resource, T2 resource2, T3 resource3, Action<T1, T2, T3> action)
            where T1 : IDisposable
            where T2 : IDisposable
            where T3 : IDisposable {
            using (resource)
            using (resource2)
            using (resource3) {
                action(resource, resource2, resource3);
            }
        }

        public static TResult Use<T, TResult>(this T resource, Func<T, TResult> action) where T : IDisposable {
            using (resource) {
                return action(resource);
            }
        }

        public static TResult Use<T1, T2, TResult>(this T1 resource, T2 resource2, Func<T1, T2, TResult> action)
            where T1 : IDisposable
            where T2 : IDisposable {
            using (resource)
            using (resource2) {
                return action(resource, resource2);
            }
        }

        public static TResult Use<T1, T2, T3, TResult>(this T1 resource, T2 resource2, T3 resource3, Func<T1, T2, T3, TResult> action)
            where T1 : IDisposable
            where T2 : IDisposable
            where T3 : IDisposable {
            using (resource)
            using (resource2)
            using (resource3) {
                return action(resource, resource2, resource3);
            }
        }

    }
}
