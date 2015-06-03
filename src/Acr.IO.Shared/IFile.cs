using System;
using System.IO;
using System.Threading.Tasks;


namespace Acr.IO {

	public interface IReadOnlyFile : IBaseFile<IReadOnlyFile, IReadOnlyDirectory> {
	}

	public interface IFile : IBaseFile<IFile, IDirectory> {

		string FullName { get; }

        Stream Create();
        Stream OpenWrite();

        void MoveTo(string path);
        void Delete();

        DateTime LastAccessTime { get; }
        DateTime LastWriteTime { get; }
        DateTime CreationTime { get; }
    }

    public interface IBaseFile<T,TD>
    	//where T:IBaseFile<T>
     	//where TD:IBaseDirectory 
    {
		string Name { get; }
        string Extension { get; }

		Uri Uri { get; }

		bool Exists { get; }
    	
		long Length { get; }
        string MimeType { get; }

		Stream OpenRead();

		IFile CopyTo(string path);
		Task<IFile> CopyToAsync(string path);

		TD Directory { get; }
    }
}
