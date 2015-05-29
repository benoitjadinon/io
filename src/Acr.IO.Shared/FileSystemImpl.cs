#if __PLATFORM__
using System;
using System.IO;
using System.Linq;


#if __IOS__
using UIKit;
using Foundation;
#endif

namespace Acr.IO {

    public class FileSystemImpl : IFileSystem {

        public FileSystemImpl() {
#if WINDOWS_PHONE
            var path = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            this.AppData = new Directory(path);
            this.Cache = new Directory(Path.Combine(path, "Cache"));
            this.Public = new Directory(Path.Combine(path, "Public"));
            this.Temp = new Directory(Path.Combine(path, "Temp"));
			this.Assets = new Directory(Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation, "Assets"));
#elif __IOS__
            var documents = UIDevice.CurrentDevice.CheckSystemVersion(8, 0)
                ? NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].Path
                : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var library = Path.Combine(documents, "..", "Library");
            this.AppData = new Directory(library);
            this.Cache = new Directory(Path.Combine(library, "Caches"));
            this.Temp = new Directory(Path.Combine(documents, "..", "tmp"));
            this.Public = new Directory(documents);
			this.Assets = new Directory(NSBundle.MainBundle.BundlePath);
#elif __ANDROID__
            try {
                var ctx = Android.App.Application.Context;
                this.AppData = new Directory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

                var ext = ctx.GetExternalFilesDir(null);
                if (ext != null)
                    this.Public = new Directory(ext.AbsolutePath);

                var cacheDir = ctx.GetExternalFilesDir(null);
                if (cacheDir != null) {
                    this.Cache = new Directory(cacheDir.AbsolutePath);
                    this.Temp = new Directory(cacheDir.AbsolutePath);
                }

				//this.Assets = new Directory("file:///android_asset"); // that's for webviews only
				this.Assets = new AndroidAssetDirectory();
            }
            finally {}
#endif
        }


		public IDirectory AppData { get; protected set; }
		public IDirectory Cache { get; protected set; }
		public IDirectory Public { get; protected set; }
        public IDirectory Temp { get; protected set; }
		public IReadOnlyDirectory Assets { get; protected set; }


        public IDirectory GetDirectory(string path) {
            return new Directory(new DirectoryInfo(path));
        }


        public IFile GetFile(string fileName) {
            return new File(new FileInfo(fileName));
        }
    }
}
#endif