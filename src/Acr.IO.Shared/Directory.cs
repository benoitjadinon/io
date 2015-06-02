﻿#if __PLATFORM__
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
            return new File(new FileInfo(path));
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

        private IEnumerable<IDirectory> directories;
        public IEnumerable<IDirectory> Directories {
            get {
                this.directories = this.directories ?? this.info.GetDirectories().Select(x => new Directory(x)).ToList();
                return this.directories;
            }
        }


        private IEnumerable<IFile> files;
        public IEnumerable<IFile> Files {
            get {
                this.files = this.files ?? this.info.GetFiles().Select(x => new File(x)).ToList();
                return this.files;
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

		public virtual IEnumerable<IReadOnlyDirectory> Directories {
			get {
				return assetManager.List(path).Where(p => assetManager.List(p).Length > 0).Select(p => new AndroidAssetDirectory(p));
			}
		}

		public virtual IEnumerable<IReadOnlyFile> Files {
			get {
				var list = assetManager.List(path).Where(p => assetManager.List(p).Length == 0);
				return list.Select(p => new AndroidAssetFile(p, path));
			}
		}

		#endregion


		protected virtual string GetFilePath (string name)
		{
			var res = assetManager.List(name);
			return res.FirstOrDefault();
		}
	}
#endif

}
#endif