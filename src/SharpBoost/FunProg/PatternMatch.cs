using System;
using System.Collections.Generic;
using System.Linq;


namespace SharpBoost.FunProg {
    public class PatternMatch<T, TResult> : IEnumerable<Tuple<Func<T, bool>, Func<T, TResult>>> {
        private T _value;

        private readonly List<Tuple<Func<T, bool>, Func<T, TResult>>> _cases
            = new List<Tuple<Func<T, bool>, Func<T, TResult>>>();

        private Func<T, TResult> _defaultFunc;

        public PatternMatch() { }

        internal void SetValue(T value) {
            _value = value;
        }

        internal PatternMatch(T value) {
            _value = value;
        }

        public void Add(Func<T, bool> predicate, Func<T, TResult> func) {
            Case(predicate)(func);
        }

        public void Add(T value, Func<T, TResult> func) {
            Case(value)(func);
        }

        public void Add(T value, TResult result) {
            Case(value, result);
        }

        public void Add(Func<T, TResult> defaultFunc) {
            Default(defaultFunc);
        }

        public void Add(TResult defaultResult) {
            Default(defaultResult);
        }

        public Func<Func<T, TResult>, PatternMatch<T, TResult>> Case(Func<T, bool> condition) {
            return f => {
                _cases.Add(Tuple.Create(condition.ArgumentNullCheck("condition"), f.ArgumentNullCheck("retFunc")));
                return this;
            };
        }

        public Func<Func<T, TResult>, PatternMatch<T, TResult>> Case(T value) {
            return Case(Lambda.F<T, bool>(x => x == null ? value == null : x.Equals(value)));
        }

        public PatternMatch<T, TResult> Case(T value, TResult result) {
            return Case(value)(_ => result);
        }

        public PatternMatch<T, TResult> Default(Func<T, TResult> defaultFunc) {
            if (_defaultFunc != null)
                throw new InvalidOperationException("Default case must declared only once.");

            _defaultFunc = defaultFunc.ArgumentNullCheck("defaultFunc");
            return this;
        }

        public PatternMatch<T, TResult> Default(TResult defaultResult) {
            return Default(_ => defaultResult);
        }

        public TResult ProcessMatch() {
            if (_defaultFunc != null)
                _cases.Add(Tuple.Create(Lambda.F<T, bool>(x => true), _defaultFunc));

            var success = _cases.FirstOrDefault(c => c.Item1(_value));
            if (success != null)
                return success.Item2(_value);

            throw new MatchNotFoundException("No one of cases was matched");
        }

        #region IEnumerable<Tuple<Func<T,bool>,Func<T,TResult>>> Members

        public IEnumerator<Tuple<Func<T, bool>, Func<T, TResult>>> GetEnumerator() {
            return _cases.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _cases.GetEnumerator();
        }

        #endregion


    }

    public static class PatternMatchExtensions {
        public static PatternMatch<T, TResult> MatchOf<T, TResult>(this T value) {
            return new PatternMatch<T, TResult>(value);
        }

        public static TResult MatchOf<T, TResult>(this T value, PatternMatch<T, TResult> matcher) {
            return matcher.ArgumentNullCheck("matcher").Do(x => x.SetValue(value)).ProcessMatch();
        }
    }

    public class MatchNotFoundException : Exception {
        public MatchNotFoundException(string message) : base(message) { }
    }
}
