﻿#if __PLATFORM__
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Acr.IO {

    public class Directory : IDirectory {
        private readonly DirectoryInfo info;


        public Directory(string path) : this(new DirectoryInfo(path)) {}
        internal Directory(DirectoryInfo info) {
            this.info = info;
        }

        #region IDirectory Members

        public string Name {
            get { return this.info.Name; }
        }


        public string FullName {
            get { return this.info.FullName; }
        }


        public bool Exists {
            get { return this.info.Exists; }
        }


        private IDirectory root;
        public IDirectory Root {
            get {
                this.root = this.root ?? new Directory(this.info.Root);
                return this.root;
            }
        }


        private IDirectory parent;
        public IDirectory Parent {
            get {
                this.parent = this.parent ?? new Directory(this.info.Parent);
                return this.parent;
            }
        }


        public DateTime CreationTime {
            get { return this.info.CreationTime; }
        }


        public DateTime LastAccessTime {
            get { return this.info.LastAccessTime; }
        }


        public DateTime LastWriteTime {
            get { return this.info.LastWriteTime; }
        }


        public void Create() {
            this.info.Create();
        }


        public void MoveTo(string path) {
            this.info.MoveTo(path);
        }


        public bool FileExists(string fileName) {
            var path = Path.Combine(this.FullName, fileName);
            return System.IO.File.Exists(path);
        }


        public IFile GetFile(string fileName) {
            var path = Path.Combine(this.FullName, fileName);
            return new File(path);
        }


        public IDirectory CreateSubdirectory(string path) {
            var dir = this.info.CreateSubdirectory(path);
            return new Directory(dir);
        }


        public void Delete(bool recursive = false) {
            this.info.Delete(recursive);
        }

		public IDirectory GetDirectory (string dirName)
		{
			return new Directory(Path.Combine(info.FullName, dirName));
		}

        public IEnumerable<IDirectory> Directories {
            get {
                return this.info.GetDirectories().Select(x => new Directory(x)).ToList();
            }
        }


        public IEnumerable<IFile> Files {
            get {
                return this.info.GetFiles().Select(x => new File(x)).ToList();
            }
        }
        #endregion
    }
}
#endif