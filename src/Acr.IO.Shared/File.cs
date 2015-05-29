#if __PLATFORM__
using System;
using System.IO;
using System.Linq;

#if __ANDROID__
using Android.App;
using Android.Content.Res;
#endif

namespace Acr.IO {

	public class File : IFile {
        private readonly FileInfo info;


        public File(string fileName) : this(new FileInfo(fileName)) {}
        internal File(FileInfo info) {
            this.info = info;
        }

        #region IFile Members

        public string Name {
            get { return this.info.Name; }
        }


        public string FullName {
            get { return this.info.FullName; }
        }


        public string Extension {
            get { return this.info.Extension; }
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


        public Stream Create() {
            return this.info.Create();
        }


        public Stream OpenRead() {
            return this.info.OpenRead();
        }


        public Stream OpenWrite() {
            return this.info.OpenWrite();
        }


        public void MoveTo(string path) {
            this.info.MoveTo(path);
        }


        public IFile CopyTo(string path) {
            var file = this.info.CopyTo(path);
            return new File(file);
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


        private string GetMimeType() {
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

#if __ANDROID__

	public class AndroidAssetFile : IReadOnlyFile {

		readonly AssetManager assetManager;

		string name;
		string path = "";

		public AndroidAssetFile (string name, string path = "")
		{
			this.path = path;
			this.name = name;
			assetManager = Application.Context.Assets;
		}

		#region IBaseFile implementation

		public Stream OpenRead ()
		{
			return assetManager.Open(name);
		}

		public IFile CopyTo (string path)
		{
			//use stream
			throw new NotImplementedException ();
		}

		public string Name {
			get {
				return name;
			}
		}

		public string Extension {
			get {
				return Path.GetExtension(this.Name);
			}
		}

		public bool Exists {
			get {				
				return assetManager.List (path).Any (p => p == this.name);
			}
		}

		public IReadOnlyDirectory Directory {
			get {
				throw new NotImplementedException ();
			}
		}


		public long Length {
			get {
				return assetManager.OpenFd(name).Length;
			}
		}

		private string mimeType;
        public string MimeType {
            get {
                this.mimeType = this.mimeType ?? GetMimeType();
                return this.mimeType;
            }
        }

		#endregion


		string GetMimeType ()
		{
			var ext = Path.GetExtension(this.Name);

			if (ext == null)
				return "*.*";

			ext = ext.ToLower().TrimStart('.');
			var mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(ext);
			return mimeType ?? "*.*";
		}
	}
#endif
}
#endif