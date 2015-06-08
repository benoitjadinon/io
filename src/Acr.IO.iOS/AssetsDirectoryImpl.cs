using System;
using System.Collections.Generic;
using System.IO;
using Foundation;

namespace Acr.IO
{
	public class IOSAssetsDirectory : IReadOnlyDirectory {

		readonly string path;

		public IOSAssetsDirectory (string path = "")
		{
			this.path = !string.IsNullOrEmpty(path) ? path : NSBundle.MainBundle.BundlePath;
			this.info = new DirectoryInfo(this.path);
		}
		public IOSAssetsDirectory (string root, string subPath)
			:this(Path.Combine(root, subPath))
		{
		}
		protected IOSAssetsDirectory (DirectoryInfo info) : this (info?.FullName)
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
			return new IOSAssetsFile(name, path);
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
			return new IOSAssetsDirectory(root:path, subPath:dirName);
		}

		private IEnumerable<IReadOnlyDirectory> directories;
		public virtual IEnumerable<IReadOnlyDirectory> Directories {
            get {
				return this.directories ?? (this.directories = Info.GetDirectories().Select(x => new IOSAssetsDirectory(x)).ToList());
            }
        }

		private IEnumerable<IReadOnlyFile> files;
		public virtual IEnumerable<IReadOnlyFile> Files {
			get {
				return this.files ?? (this.files = Info.GetFiles().Select(x => new IOSAssetsFile(x)).ToList());
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
