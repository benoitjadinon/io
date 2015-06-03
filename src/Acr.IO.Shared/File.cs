#if __PLATFORM__
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

#if __ANDROID__
using Android.App;
using Android.Content.Res;
#elif __IOS__
using Foundation;
#endif

namespace Acr.IO {

	public abstract class AbstractReadOnlyFile : IReadOnlyFile
	{
		protected readonly FileInfo info;

		internal AbstractReadOnlyFile(string fileName) : this(new FileInfo(fileName)) {}
		internal AbstractReadOnlyFile(FileInfo info) {
            this.info = info;
        }

		#region IBaseFile implementation

        public Stream OpenRead() {
            return this.info.OpenRead();
        }

		public IFile CopyTo (string path)
		{
			var file = new File (path);
			using (Stream readStream = OpenRead()) {
				using (Stream writeStream = file.Create ()) {
					readStream.CopyTo(writeStream);
				}
		    }
		    return file;
		}
		public async Task<IFile> CopyToAsync (string path)
		{
			var file = new File (path);
			using (Stream readStream = OpenRead()) {
				using (Stream writeStream = file.Create ()) {
					await readStream.CopyToAsync(writeStream);
				}
		    }
		    return file;
		}

		public string Name {
            get { return this.info.Name; }
        }

		public string Extension {
			get {
				return Path.GetExtension (this.Name);
				//return this.info.Extension;
			}
        }


        private string mimeType;
        public string MimeType {
            get {
                this.mimeType = this.mimeType ?? GetMimeType();
                return this.mimeType;
            }
        }

        public long Length {
            get { return this.info.Length; }
        }


        public bool Exists {
            get { return this.info.Exists; }
        }

		public IReadOnlyDirectory Directory {
			get {
				throw new NotImplementedException();
			}
		}

		public Uri Uri {
			get {
				return new UriBuilder("file", info.FullName).Uri;
			}
		}

		#endregion

		protected virtual string GetMimeType() {
			var ext = Path.GetExtension(this.Name);
//			if (ext == null)
//				return String.Empty;
//
//			switch (ext.ToLower()) {
//			case ".jpg" : return "image/jpg";
//			case ".png" : return "image/png";
//			case ".gif" : return "image/gif";
//			case ".pdf" : return "application/pdf";
//			case ".docs": return "application/";
//			}
            return String.Empty;
        }
	}

	public class File : AbstractReadOnlyFile, IFile {

        public File(string fileName) : base(fileName) {}
		internal File(FileInfo info) : base(info) {
			
		}

        #region IFile Members

		public string FullName {
            get { return this.info.FullName; }
        }


        public Stream Create() {
            return this.info.Create();
        }

        public Stream OpenWrite() {
            return this.info.OpenWrite();
        }


        public void MoveTo(string path) {
            this.info.MoveTo(path);
        }

        public void Delete() {
            this.info.Delete();
        }


        private Directory directory; 
        public IDirectory Directory {
            get {
                this.directory = this.directory ?? new Directory(this.info.Directory);
                return this.directory;
            }
        }


        public DateTime LastAccessTime {
            get { return this.info.LastAccessTime; }
        }


        public DateTime LastWriteTime {
            get { return this.info.LastWriteTime; }
        }


        public DateTime CreationTime {
            get { return this.info.CreationTime; }
        }

        #endregion
    }

#if __ANDROID__

	public class AndroidAssetFile : AbstractReadOnlyFile {

		readonly AssetManager assetManager;

		string name;
		string path = "";

		public AndroidAssetFile (string name):base(name)
		{
			assetManager = Application.Context.Assets;
			this.name = name;
		}
		public AndroidAssetFile (string name, string path):this(name)
		{
			this.path = path;
		}

		#region IBaseFile implementation

		public override Stream OpenRead ()
		{
			return assetManager.Open(FullName);
		}

		public override string Name {
			get {
				return name;
			}
		}

		public override bool Exists {
			get {				
				return assetManager.List (path).Any (p => p == this.name);
			}
		}

		public override IReadOnlyDirectory Directory {
			get {
				throw new NotImplementedException ();
			}
		}


		public override long Length {
			get {
				return assetManager.OpenFd(name).Length;
			}
		}

		public override Uri Uri {
			get {
				//TODO: not sure, file:///android_asset is only usable by a webview, is it the only purpose of .Uri ?
				//https://android.googlesource.com/platform/frameworks/base.git/+/android-4.2.2_r1/core/java/android/webkit/URLUtil.java
				var urib = new UriBuilder("file", "/android_asset");
				urib.Path = FullName;
				return urib.Uri;
			}
		}

		#endregion

		public string FullName {
			get {
				return Path.Combine(path, name);
			}
		}

		protected override string GetMimeType ()
		{
			var ext = Path.GetExtension(this.Name);

			if (ext == null)
				return "*.*";

			ext = ext.ToLower().TrimStart('.');
			var mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(ext);
			return mimeType ?? "*.*";
		}
	}

#elif __IOS__

	public class IOSAssetsFile : AbstractReadOnlyFile {

		public IOSAssetsFile (string name, string path = "")
			:base(Path.Combine(path ?? NSBundle.MainBundle.BundlePath, name))
		{
		}

		public IOSAssetsFile (FileInfo info) : base (info)
		{
		}
	}

#endif


}
#endif