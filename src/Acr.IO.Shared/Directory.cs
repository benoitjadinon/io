#if __PLATFORM__
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Content.Res;
using Env = Android.OS.Environment;
#endif
#if __IOS__
using Foundation;
using UIKit;
#endif

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

		public IDirectory GetSubDirectory (string dirName)
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

#if __ANDROID__

	public class AndroidAssetDirectory : IReadOnlyDirectory {

		//TODO: parent support
		//TODO: lazy?
		readonly AssetManager assetManager;

		string path;

		public AndroidAssetDirectory (string path = "")
		{
			this.path = path;
			assetManager = Application.Context.Assets;
		}
		public AndroidAssetDirectory (string root, string subPath)
			:this(Path.Combine(root, subPath))
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
			return new AndroidAssetFile(name, path);
		}

		public virtual string Name {
			get {
				return path;
			}
		}

		public string FullName {
			get {
				return path;
			}
		}

		public virtual bool Exists {
			get {
				// TODO: list parent and search for itself
				return true;
			}
		}

		public IReadOnlyDirectory GetSubDirectory (string dirName)
		{
			return new AndroidAssetDirectory(root:path, subPath:dirName);
		}

		private IEnumerable<IReadOnlyDirectory> directories;
		public virtual IEnumerable<IReadOnlyDirectory> Directories {
			get {
				return directories ?? (directories = assetManager.List(path).Where(p => assetManager.List(p).Length > 0)
					.Select(p => new AndroidAssetDirectory(p)));
			}
		}

		private IEnumerable<IReadOnlyFile> files;
		public virtual IEnumerable<IReadOnlyFile> Files {
			get {
				return files ?? (files = assetManager.List(path).Where(p => assetManager.List(p).Length == 0)
					.Select(p => new AndroidAssetFile(p, path)));
			}
		}

		#endregion
	}

#elif __IOS__

	public class IOSAssetsDirectory : IReadOnlyDirectory {

		string path;

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

		public IReadOnlyDirectory GetSubDirectory (string dirName)
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
#endif

}
#endif