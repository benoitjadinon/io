using System;
using System.IO;
using Foundation;

namespace Acr.IO
{
	public class IOSAssetFileImpl : AbstractReadOnlyFile {

		public IOSAssetFileImpl (string name, string path = "")
			:base(Path.Combine(path ?? NSBundle.MainBundle.BundlePath, name))
		{
		}

		public IOSAssetFileImpl (FileInfo info) : base (info)
		{
		}

		#region implemented abstract members of AbstractReadOnlyFile

		public override IReadOnlyDirectory Directory {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion
	}
}

