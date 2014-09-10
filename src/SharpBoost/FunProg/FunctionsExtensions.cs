using System;
using System.Threading;

namespace SharpBoost.FunProg {
    // ReSharper disable InconsistentNaming
    public static class FunctionsExtensions {
        #region Action to Func conversion

        public static Func<Unit> ToFunc(this Action action) {
            return () => {
                action();
                return new Unit();
            };
        }

        public static Func<T, Unit> ToFunc<T>(this Action<T> action) {
            return t => {
                action(t);
                return new Unit();
            };
        }

        public static Func<T, T1, Unit> ToFunc<T, T1>(this Action<T, T1> action) {
            return (t, t1) => {
                action(t, t1);
                return new Unit();
            };
        }

        public static Func<T, T1, T2, Unit> ToFunc<T, T1, T2>(this Action<T, T1, T2> action) {
            return (t, t1, t2) => {
                action(t, t1, t2);
                return new Unit();
            };
        }

        public static Func<T, T1, T2, T3, Unit> ToFunc<T, T1, T2, T3>(this Action<T, T1, T2, T3> action) {
            return (t, t1, t2, t3) => {
                action(t, t1, t2, t3);
                return new Unit();
            };
        }

#if NET4
        public static Func<T, T1, T2, T3, T4, Unit> ToFunc<T, T1, T2, T3, T4>(this Action<T, T1, T2, T3, T4> action) {
            return (t, t1, t2, t3, t4) => {
                action(t, t1, t2, t3, t4);
                return new Unit();
            };
        }
#endif

        public static Func<T, TResult> ToFunc<T, TResult>(this Action<T> action, TResult value) {
            return t => {
                action(t);
                return value;
            };
        }

        #endregion

        #region Execute with tries
        private static void InternalWait(int milliseconds) {
            using (var evnt = new ManualResetEvent(false))
                evnt.WaitOne(milliseconds);
        }


        public static Action ExecuteWithTries(this Action action, int tries, int period) {
            return () => {
                while (tries > 0) {
                    try {
                        action();
                        return;
                    }
                    catch {
                        tries--;
                        if (tries <= 0)
                            throw;
                        if (period > 0)
                            InternalWait(period);
                    }
                }
            };
        }

        public static Func<T> ExecuteWithTries<T>(this Func<T> action, int tries, int period) {
            return () => {
                while (tries > 0) {
                    try {
                        return action();
                    }
                    catch {
                        tries--;
                        if (tries <= 0)
                            throw;
                        if (period > 0)
                            InternalWait(period);
                    }
                }

                throw new TimeoutException();
            };
        }

        #endregion

        #region TryCatch

        public static Action TryCatch(this Action action, Action<Exception> catchBlock = null) {
            return () => {
                try {
                    action();
                }
                catch (Exception ex) {
                    if (catchBlock != null)
                        catchBlock(ex);
                }
            };
        }

        public static Action<T> TryCatch<T>(this Action<T> action, Action<Exception> catchBlock = null) {
            return item => {
                try {
                    action(item);
                }
                catch (Exception ex) {
                    if (catchBlock != null)
                        catchBlock(ex);
                }
            };
        }

        public static Action<T> TryCatch<T>(this Action<T> action, Action<Exception, T> catchBlock = null) {
            return item => {
                try {
                    action(item);
                }
                catch (Exception ex) {
                    if (catchBlock != null)
                        catchBlock(ex, item);
                }
            };
        }

        #endregion

        #region Control flow

        public static Action AddAfter(this Action action, Action next) {
            next.ArgumentNullCheck("next");

            return () => {
                action();
                next();
            };
        }

        public static Action<T> AddAfter<T>(this Action<T> action, Action<T> next) {
            next.ArgumentNullCheck("next");

            return t => {
                action(t);
                next(t);
            };
        }


        public static Action<T, U> AddAfter<T, U>(this Action<T> action, Action<U> next) {
            next.ArgumentNullCheck("next");

            return (t, u) => {
                action(t);
                next(u);
            };
        }

        public static Action<T, T2> AddAfter<T, T2>(this Action<T, T2> action, Action<T, T2> next) {
            next.ArgumentNullCheck("next");

            return (t, t2) => {
                action(t, t2);
                next(t, t2);
            };
        }

        public static Action AddBefore(this Action action, Action before) {
            before.ArgumentNullCheck("before");

            return () => {
                before();
                action();
            };
        }

        public static Action<T> AddBefore<T>(this Action<T> action, Action<T> before) {
            before.ArgumentNullCheck("before");

            return t => {
                before(t);
                action(t);
            };
        }

        public static Action<T, U> AddBefore<T, U>(this Action<T> action, Action<U> before) {
            before.ArgumentNullCheck("before");

            return (t, u) => {
                before(u);
                action(t);
            };
        }

        public static Action<T, T2> AddBefore<T, T2>(this Action<T, T2> action, Action<T, T2> before) {
            before.ArgumentNullCheck("before");

            return (t, t2) => {
                before(t, t2);
                action(t, t2);
            };
        }

        #endregion

    }
}
