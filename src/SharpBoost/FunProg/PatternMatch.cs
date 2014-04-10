using System;
using System.Collections.Generic;
using System.Linq;


namespace SharpBoost.FunProg {
    public class PatternMatch<T, TResult> {
        private readonly T _value;

        private readonly List<Tuple<Func<T, bool>, Func<T, TResult>>> _cases
            = new List<Tuple<Func<T, bool>, Func<T, TResult>>>();

        private Func<T, TResult> _defaultFunc;

        internal PatternMatch(T value) {
            _value = value;
        }

        public Func<Func<T, TResult>, PatternMatch<T, TResult>> Case(Func<T, bool> condition) {
            return f => {
                _cases.Add(Tuple.Create(condition.ArgumentNullCheck("condition"), f.ArgumentNullCheck("retFunc")));
                return this;
            };
        }

        public Func<Func<T, TResult>, PatternMatch<T, TResult>> Case(T value) {
            return f => {
                _cases.Add(Tuple.Create(Lambda.F<T, bool>(x => x.Equals(value)), f.ArgumentNullCheck("retFunc")));
                return this;
            };
        }

        public PatternMatch<T, TResult> Default(Func<T, TResult> defaultFunc) {
            if (_defaultFunc != null)
                throw new InvalidOperationException("Default case must declared only once.");

            _defaultFunc = defaultFunc.ArgumentNullCheck("defaultFunc");
            return this;
        }

        public TResult ProcessMatch() {
            if (_defaultFunc != null)
                _cases.Add(Tuple.Create(Lambda.F<T, bool>(x => true), _defaultFunc));

            foreach (var c in _cases.Where(c => c.Item1(_value))) { return c.Item2(_value); }

            throw new MatchNotFoundException("No one of cases was matched");
        }
    }

    public static class PatternMatchExtensions {
        public static PatternMatch<T, TResult> MatchOf<T, TResult>(this T value) {
            return new PatternMatch<T, TResult>(value);
        }
    }

    public class MatchNotFoundException : Exception {
        public MatchNotFoundException(string message) : base(message) { }
    }
}
