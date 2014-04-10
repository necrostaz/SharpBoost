using System;
using System.IO;
using System.Text;
using System.Xml;

namespace SharpBoost {
    public static class StreamExtensions {
        public static void Reset(this Stream source) {
            source.ArgumentNullCheck("source");
            if (source.CanSeek)
                source.Seek(0, SeekOrigin.Begin);
        }

        public static void CopyTo(this Stream source, Stream target) {
            CopyTo(source, target, 0x1000);
        }

        public static void CopyTo(this Stream source, Stream target, int bufferLength) {
            source.ArgumentNullCheck("source");
            target.ArgumentNullCheck("target");

            var buffer = new byte[bufferLength];
            int count;
            source.Reset();
            do {
                count = source.Read(buffer, 0, buffer.Length);
                target.Write(buffer, 0, count);
            }
            while (count > 0);
        }

        public static string ToTempFile(this Stream stream) {
            stream.ArgumentNullCheck("stream");

            var tmpFileName = Path.GetTempFileName();
            ToFile(stream, tmpFileName);

            return tmpFileName;
        }

        public static void ToFile(this Stream stream, string path) {
            stream.ArgumentNullCheck("stream");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("path is null or empty");

            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                stream.CopyTo(fs);
        }

        public static MemoryStream ToMemoryStream(this Stream stream) {
            return new MemoryStream()
                .Do(stream.ArgumentNullCheck("stream").CopyTo)
                .Do(s => s.Position = 0);
        }

        public static byte[] ReadAsByteArray(this Stream stream) {
            stream.ArgumentNullCheck("stream");

            using (var ms = new MemoryStream()) {
                CopyTo(stream, ms);
                return ms.ToArray();
            }
        }

        public static string ReadAsBase64(this Stream stream) {
            return Convert.ToBase64String(stream.ArgumentNullCheck("stream").ReadAsByteArray());
        }

        public static string ReadAsString(this Stream stream, Encoding encoding = null) {
            stream.ArgumentNullCheck("stream");

            var reader = encoding != null
                ? new StreamReader(stream, encoding)
                : new StreamReader(stream);

            return reader.ReadToEnd();
        }

        public static XmlDocument ReadAsXmlDocument(this Stream stream) {
            stream.ArgumentNullCheck("stream");


            var document = new XmlDocument {
                PreserveWhitespace = true
            };

            document.LoadXml(stream.ReadAsString());

            return document;
        }
    }
}
