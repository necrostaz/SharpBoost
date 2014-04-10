namespace SharpBoost.FunProg {
    public sealed class Unit {
        public override bool Equals(object obj) {
            return obj == null || obj is Unit;
        }

        public override int GetHashCode() {
            return 0;
        }
    }
}
