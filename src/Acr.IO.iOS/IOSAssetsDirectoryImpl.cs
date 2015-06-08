using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Foundation;

namespace Acr.IO
{
	public class IOSAssetsDirectoryImpl : IReadOnlyDirectory {

		readonly string path;

		public IOSAssetsDirectoryImpl (string path = "")
		{
			this.path = !string.IsNullOrEmpty(path) ? path : NSBundle.MainBundle.BundlePath;
			this.info = new DirectoryInfo(this.path);
		}
		public IOSAssetsDirectoryImpl (string root, string subPath)
			:this(Path.Combine(root, subPath))
		{
		}
		protected IOSAssetsDirectoryImpl (DirectoryInfo info) : this (info?.FullName)
		{
		}

		#region IDirectory implementation

		public virtual bool FileExists (string fileName)
		{
			var file = GetFile(fileName);
			return file != null && file.Exists;
		}

		public virtual IReadOnlyFile GetFile (string name)
		{
			return new IOSAssetFileImpl(name, path);
		}

		public virtual string Name {
			get {
				return this.info.Name;
			}
		}

		public string FullName {
			get {
				return this.info.FullName;
			}
		}

		public virtual bool Exists {
			get {
				return true;
			}
		}

		public IReadOnlyDirectory GetDirectory (string dirName)
		{
			return new IOSAssetsDirectoryImpl(root:path, subPath:dirName);
		}

		private IEnumerable<IReadOnlyDirectory> directories;
		public virtual IEnumerable<IReadOnlyDirectory> Directories {
            get {
				return this.directories ?? (this.directories = Info.GetDirectories().Select(x => new IOSAssetsDirectoryImpl(x)).ToList());
            }
        }

		private IEnumerable<IReadOnlyFile> files;
		public virtual IEnumerable<IReadOnlyFile> Files {
			get {
				return this.files ?? (this.files = Info.GetFiles().Select(x => new IOSAssetFileImpl(x)).ToList());
			}
		}

		private DirectoryInfo info;
		protected DirectoryInfo Info {
			get {
				return this.info ?? (this.info = new DirectoryInfo(path));
			}
		}
		#endregion
	}
}
