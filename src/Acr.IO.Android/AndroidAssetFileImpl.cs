using System;
using System.Linq;
using Android.Content.Res;
using Android.App;
using System.IO;

namespace Acr.IO
{
	public class AndroidAssetFileImpl : AbstractReadOnlyFile 
	{
		readonly AssetManager assetManager;

		string name;
		string path = "";

		public AndroidAssetFileImpl (string name):base(name)
		{
			assetManager = Application.Context.Assets;
			this.name = name;
		}
		public AndroidAssetFileImpl (string name, string path):this(name)
		{
			this.path = path;
		}

		#region AbstractReadOnlyFile implementation

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
}

