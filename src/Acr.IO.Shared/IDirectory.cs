using System;
using System.Collections.Generic;


namespace Acr.IO {

	public interface IReadOnlyDirectory : IBaseDirectory<IReadOnlyDirectory, IReadOnlyFile> {
	}

	public interface IDirectory : IBaseDirectory<IDirectory, IFile> {

        string FullName { get; }

        void Create();
        void MoveTo(string path);
        void Delete(bool recursive = false);

        DateTime CreationTime { get; }
        DateTime LastAccessTime { get; }
        DateTime LastWriteTime { get; }

        IDirectory CreateSubdirectory(string name);

		IDirectory Root { get; }
        IDirectory Parent { get; }
    }

    public interface IBaseDirectory<T, TF> {

		string Name { get; }

		bool Exists { get; }

		bool FileExists(string fileName);

		T GetSubDirectory(string dirName);
		IEnumerable<T> Directories { get; }

		TF GetFile(string name);
		IEnumerable<TF> Files { get; }
    }
}
