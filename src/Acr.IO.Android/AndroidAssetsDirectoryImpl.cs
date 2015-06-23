using System;
using System.Linq;
using Android.Content.Res;
using Android.App;
using System.IO;
using System.Collections.Generic;

namespace Acr.IO
{
	public class AndroidAssetsDirectoryImpl : IReadOnlyDirectory {

		//TODO: parent support, then exists
		//TODO: lazy?
		readonly AssetManager assetManager;

		readonly string path;

		public AndroidAssetsDirectoryImpl (string path = "")
		{
			this.path = path;
			assetManager = Application.Context.Assets;
		}
		protected AndroidAssetsDirectoryImpl (string name, string rootName)
			:this(rootName != "" ? Path.Combine(rootName, name) : name)
		{
		}

		#region IReadOnlyDirectory implementation

		public IReadOnlyDirectory GetRoot ()
		{
			return new AndroidAssetsDirectoryImpl("");
		}

		public virtual bool FileExists (string fileName)
		{
			var file = GetFile(fileName);
			return file != null && file.Exists;
		}

		public virtual IReadOnlyFile GetFile (string name)
		{
			if (path != "")
				return new AndroidAssetFileImpl(name, path);
			else
				return new AndroidAssetFileImpl(name);
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
				if (string.IsNullOrEmpty (path))
					return true;
				else {
					string parentPath = ""; // root by default
					if (path.Contains (Path.DirectorySeparatorChar)) {
						parentPath = path.Substring (0, path.LastIndexOf (Path.DirectorySeparatorChar));
					}
					return new AndroidAssetsDirectoryImpl (parentPath).Directories.Any (p => p.Name == this.Name);
				}
			}
		}

		public IReadOnlyDirectory GetDirectory (string dirName)
		{
			return new AndroidAssetsDirectoryImpl(name:path, rootName:dirName);
		}

		private IEnumerable<IReadOnlyDirectory> directories;
		public virtual IEnumerable<IReadOnlyDirectory> Directories {
			get {
				return directories ?? (directories = assetManager.List(path).Where(p => assetManager.List(p).Length > 0)
					.Select(p => new AndroidAssetsDirectoryImpl(p)));
			}
		}

		private IEnumerable<IReadOnlyFile> files;
		public virtual IEnumerable<IReadOnlyFile> Files {
			get {
				return files ?? (files = assetManager.List(path).Where(p => assetManager.List(p).Length == 0)
					.Select(p => new AndroidAssetFileImpl(p, path)));
			}
		}

		#endregion 
	}
}