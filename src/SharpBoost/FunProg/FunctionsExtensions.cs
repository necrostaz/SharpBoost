using System;
using System.Threading;

namespace SharpBoost.FunProg {
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

    }
}
