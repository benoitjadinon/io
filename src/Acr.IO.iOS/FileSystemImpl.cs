using System;
using UIKit;
using Foundation;
using System.IO;

namespace Acr.IO
{
	public partial class FileSystemImpl
	{
		public FileSystemImpl ()
		{
			var documents = UIDevice.CurrentDevice.CheckSystemVersion(8, 0)
                ? NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].Path
                : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var library = Path.Combine(documents, "..", "Library");

            this.AppData = new Directory(library);
            this.Cache = new Directory(Path.Combine(library, "Caches"));
            this.Temp = new Directory(Path.Combine(documents, "..", "tmp"));
            this.Public = new Directory(documents);

			this.Assets = new IOSAssetsDirectoryImpl();
		}
	}
}

