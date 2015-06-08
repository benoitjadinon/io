using System;

namespace Acr.IO
{
	public partial class FileSystemImpl
	{
		public FileSystemImpl ()
		{
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

			this.Assets = new AndroidAssetsDirectoryImpl();
		}
	}
}

