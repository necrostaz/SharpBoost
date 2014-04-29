using System.IO;

namespace SharpBoost {
    public static class ArrayExtensions {
        public static Stream ToStream(this byte[] bytes) {
            return new MemoryStream(bytes.ArgumentNullCheck("bytes"));
        }
    }
}
