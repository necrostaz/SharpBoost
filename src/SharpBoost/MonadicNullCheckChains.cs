using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI.WebControls;

namespace SharpBoost {
    public static class MonadicNullCheckChains {
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class {
            return o == null ? null : evaluator(o);
        }

        public static T ValOrDefault<T>(this T o, T defaultValue)
        where T : class {
            return o ?? defaultValue;
        }

        public static TInput ValOrError<TInput, TError>(this TInput o, Func<TError> error)
            where TInput : class
            where TError : Exception {
            if (o == null)
                throw error();
            return o;
        }

        public static TInput ValOrError<TInput>(this TInput o, Exception error)
            where TInput : class {
            return ValOrError(o, () => error);
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult defaultValue)
            where TInput : class {
            return o == null ? defaultValue : evaluator(o);
        }

        public static TResult ReturnOrError<TInput, TResult, TError>(this TInput o,
            Func<TInput, TResult> evaluator, Func<TError> error)
            where TInput : class
            where TError : Exception {
            if (o == null)
                throw error();
            return evaluator(o);
        }

        public static TResult ReturnOrError<TInput, TResult>(this TInput o,
            Func<TInput, TResult> evaluator, Exception error)
            where TInput : class {
            return ReturnOrError(o, evaluator, () => error);
        }

        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class {
            if (o == null) return null;
            return evaluator(o) ? o : null;
        }

        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class {
            if (o == null) return null;
            return evaluator(o) ? null : o;
        }

        public static TInput Do<TInput>(this TInput o, Action<TInput> action)
            where TInput : class {
            if (o == null) return null;
            action(o);
            return o;
        }

        public static Tuple<T1, T2> TupleWith<T1, T2>(this T1 o1, T2 o2)
            where T1 : class
            where T2 : class {
            return o1 == null || o2 == null ? null : new Tuple<T1, T2>(o1, o2);
        }

    }
}
