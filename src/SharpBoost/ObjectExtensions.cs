
namespace SharpBoost {
    public static class ObjectExtensions {
        public static T CastTo<T>(this object o) {
            return (T) o;
        }

        public static T CastAs<T>(this object o) where T : class{
            return o as T;
        }
    }
}
