#if !NET4
namespace SharpBoost {
    public class Tuple<T1, T2> {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        internal Tuple(T1 item1, T2 item2) {
            Item1 = item1;
            Item2 = item2;
        }
    }

    public class Tuple<T1, T2, T3> {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        public T3 Item3 { get; private set; }
        internal Tuple(T1 item1, T2 item2, T3 item3) {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }
    }

    public static class Tuple {
        public static Tuple<T1, T2> Create<T1, T2>(T1 i1, T2 i2) {
            return new Tuple<T1, T2>(i1, i2);
        }

        public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 i1, T2 i2, T3 i3) {
            return new Tuple<T1, T2, T3>(i1, i2, i3);
        }
    }
}
#endif