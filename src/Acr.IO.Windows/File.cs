using System;
using System.IO;
using Windows.Storage;


namespace Acr.IO {

    public class File : IFile {
        private readonly StorageFile file;


        internal File(StorageFile file) {
            this.file = file;
        }


        public string Name {
            get { return this.file.Name; }
        }


        public string FullName {
            get { return this.file.Path; }
        }


        public string Extension {
            get { return Path.GetExtension(this.file.Name); }
        }


        public long Length {
            get { return -1; }
        }

        public bool Exists {
            get { throw new NotImplementedException(); }
        }

        public string MimeType {
            get { throw new NotImplementedException(); }
        }

        public System.IO.Stream Create() {
            throw new NotImplementedException();
        }

        public System.IO.Stream OpenRead() {
            throw new NotImplementedException();
        }

        public System.IO.Stream OpenWrite() {
            throw new NotImplementedException();
        }

        public void MoveTo(string path) {
            throw new NotImplementedException();
        }

        public IFile CopyTo(string path) {
            throw new NotImplementedException();
        }

        public void Delete() {
            throw new NotImplementedException();
        }

        public IDirectory Directory {
            get { throw new NotImplementedException(); }
        }

        public DateTime LastAccessTime {
            get { throw new NotImplementedException(); }
        }

        public DateTime LastWriteTime {
            get { throw new NotImplementedException(); }
        }

        public DateTime CreationTime {
            get { throw new NotImplementedException(); }
        }
    }
}
