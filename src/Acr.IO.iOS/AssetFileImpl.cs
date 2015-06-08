using System;
using System.IO;
using Foundation;

namespace Acr.IO
{
	public class AssetFileImpl : AbstractReadOnlyFile {

		public AssetFileImpl (string name, string path = "")
			:base(Path.Combine(path ?? NSBundle.MainBundle.BundlePath, name))
		{
		}

		public AssetFileImpl (FileInfo info) : base (info)
		{
		}
	}
}

