using System;
using System.Collections.Generic;
using Windows.Storage;


namespace Acr.IO {

    public class Directory : IDirectory {
        private readonly IStorageFolder folder;


        internal Directory(IStorageFolder folder) {
            this.folder = folder;
        }


        public string Name {
            get { return this.folder.Name; }
        }


        public string FullName {
            get { return this.folder.Path; }
        }


        public bool Exists {
            get { throw new NotImplementedException(); }
        }


        public IDirectory Root {
            get { throw new NotImplementedException(); }
        }


        public IDirectory Parent {
            get { return null; }
        }


        public DateTime CreationTime {
            get { throw new NotImplementedException(); }
        }

        public DateTime LastAccessTime {
            get { throw new NotImplementedException(); }
        }

        public DateTime LastWriteTime {
            get { throw new NotImplementedException(); }
        }

        public void Create() {
            throw new NotImplementedException();
        }

        public void MoveTo(string path) {
            throw new NotImplementedException();
        }

        public void Delete(bool recursive = false) {
            throw new NotImplementedException();
        }

        public bool FileExists(string fileName) {
            throw new NotImplementedException();
        }

        public IFile CreateFile(string name) {
            throw new NotImplementedException();
        }

        public IDirectory CreateSubdirectory(string name) {
            throw new NotImplementedException();
        }

        public IEnumerable<IDirectory> Directories {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IFile> Files {
            get { throw new NotImplementedException(); }
        }
    }
}
