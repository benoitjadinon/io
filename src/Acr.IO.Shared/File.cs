#if __PLATFORM__
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Acr.IO {

	public abstract class AbstractReadOnlyFile : IReadOnlyFile
	{
		protected readonly FileInfo Info;

		internal AbstractReadOnlyFile(string fileName) : this(new FileInfo(fileName)) {}
		internal AbstractReadOnlyFile(FileInfo info) {
            this.Info = info;
        }

		#region IBaseFile implementation

		public virtual Stream OpenRead() {
            return this.Info.OpenRead();
        }

		public virtual IFile CopyTo (string path)
		{
			var file = new File (path);
			using (Stream readStream = OpenRead()) {
				using (Stream writeStream = file.Create ()) {
					readStream.CopyTo(writeStream);
				}
		    }
		    return file;
		}
		public virtual async Task<IFile> CopyToAsync (string path)
		{
			var file = new File (path);
			using (Stream readStream = OpenRead()) {
				using (Stream writeStream = file.Create ()) {
					await readStream.CopyToAsync(writeStream);
				}
		    }
		    return file;
		}

		public virtual string Name {
            get { return this.Info.Name; }
        }

		public virtual string Extension {
			get {
				return Path.GetExtension (this.Name);
				//return this.info.Extension;
			}
        }


        private string mimeType;
		public virtual string MimeType {
            get {
                this.mimeType = this.mimeType ?? GetMimeType();
                return this.mimeType;
            }
        }

		public virtual long Length {
            get { return this.Info.Length; }
        }


		public virtual bool Exists {
            get { return this.Info.Exists; }
        }

		public virtual IReadOnlyDirectory Directory {
			get {
				throw new NotImplementedException();
			}
		}

		public virtual Uri Uri {
			get {
				return new UriBuilder("file", Info.FullName).Uri;
			}
		}

		#endregion

		protected virtual string GetMimeType() {
			var ext = Path.GetExtension(this.Name);
			//TODO
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
            get { return this.Info.FullName; }
        }


        public Stream Create() {
            return this.Info.Create();
        }

        public Stream OpenWrite() {
            return this.Info.OpenWrite();
        }


        public void MoveTo(string path) {
            this.Info.MoveTo(path);
        }

        public void Delete() {
            this.Info.Delete();
        }


        private Directory directory; 
        public IDirectory Directory {
            get {
                this.directory = this.directory ?? new Directory(this.Info.Directory);
                return this.directory;
            }
        }


        public DateTime LastAccessTime {
            get { return this.Info.LastAccessTime; }
        }


        public DateTime LastWriteTime {
            get { return this.Info.LastWriteTime; }
        }


        public DateTime CreationTime {
            get { return this.Info.CreationTime; }
        }

        #endregion
    }


}
#endif