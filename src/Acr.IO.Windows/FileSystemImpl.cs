using System;
using Windows.Storage;


namespace Acr.IO {

    public partial class FileSystemImpl {

        public FileSystemImpl() {
            //var path = ApplicationData.Current.LocalFolder.Path;
            //this.AppData = new Directory(path);
            //this.Cache = new Directory(Path.Combine(path, "Cache"));
            //this.Public = new Directory(Path.Combine(path, "Public"));
            //this.Temp = new Directory(Path.Combine(path, "Temp"));
            //StorageFolder.GetFolderFromPathAsync("").AsTask().ContinueWith(x => {
            //});
			var path = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            this.AppData = new Directory(path);
            this.Cache = new Directory(Path.Combine(path, "Cache"));
            this.Public = new Directory(Path.Combine(path, "Public"));
            this.Temp = new Directory(Path.Combine(path, "Temp"));
			this.Assets = new Directory(Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation, "Assets"));
        }
    }
}
