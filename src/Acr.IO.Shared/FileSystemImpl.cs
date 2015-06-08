#if __PLATFORM__
using System;
using System.IO;
using System.Linq;

namespace Acr.IO {

    public partial class FileSystemImpl : IFileSystem {

		public IDirectory AppData { get; protected set; }
		public IDirectory Cache { get; protected set; }
		public IDirectory Public { get; protected set; }
        public IDirectory Temp { get; protected set; }
		public IReadOnlyDirectory Assets { get; protected set; }

		/*
        public IDirectory GetDirectory(string path) {
            return new Directory(new DirectoryInfo(path));
        }


        public IFile GetFile(string fileName) {
            return new File(new FileInfo(fileName));
        }
        */
    }
}
#endif