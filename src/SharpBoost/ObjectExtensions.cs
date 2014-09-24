namespace SharpBoost {
    public static class ObjectExtensions {
        public static T CastTo<T>(this object o) {
            return (T) o;
        }
    }
}
