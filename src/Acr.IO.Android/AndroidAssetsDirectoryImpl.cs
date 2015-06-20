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
		protected AndroidAssetsDirectoryImpl (string root, string subPath)
			:this(root != "" ? Path.Combine(root, subPath) : subPath)
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
				return new AndroidAssetFileImpl(path, name);
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
				// TODO: list parent and search for itself
				return true;
			}
		}

		public IReadOnlyDirectory GetDirectory (string dirName)
		{
			return new AndroidAssetsDirectoryImpl(root:path, subPath:dirName);
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