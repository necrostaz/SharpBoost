using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpBoost.IO {
    public class TempFile : IDisposable {
        private readonly string _path;
        private readonly object _sync = new object();

        public TempFile() : this(Path.GetTempFileName()) { }

        public TempFile(string path) {
            _path = path.StringArgumentCheck("path")
                .ArgumentCheck(_ => !File.Exists(path))
                ("File {0} doesn't exist".F(path));
        }

        public void ProcessPath(Action<string> foo) {
            lock (_sync)
                 foo.ArgumentNullCheck("foo")(_path);
        }

        public T ProcessPath<T>(Func<string, T> foo) {
            lock (_sync)
                return foo.ArgumentNullCheck("foo")(_path);
        }

        #region IDisposable Members

        void IDisposable.Dispose() {
            lock (_sync) {
                if (!File.Exists(_path))
                    return;
                File.Delete(_path);
            }
        }

        #endregion
    }
}
