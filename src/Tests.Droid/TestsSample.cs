using System;
using NUnit.Framework;
using Acr.IO;

namespace Tests
{
	[TestFixture]
	public class TestsSample
	{
		const string RootAssetFile = "Default.png";

		const string SubDirectoryName = "SubFolder";

		const string SubDirectoryFile = "Icon.png";


		IReadOnlyDirectory ass;

		
		[SetUp]
		public void Setup ()
		{
			ass = FileSystem.Instance.Assets;
		}

		
		[TearDown]
		public void Tear ()
		{
		}

		[Test]
		public void AssetsDirectoryExists ()
		{
			Assert.True (ass.Exists);
		}

		[Test]
		public void AssetFileExists ()
		{
			Assert.True(ass.FileExists(RootAssetFile));
		}
		[Test]
		public void AssetFileNotExists ()
		{
			Assert.False(ass.FileExists("NonExistingFile.png"));
		}

		[Test]
		public void AssetSubDirectoryExists ()
		{
			Assert.True(ass.GetDirectory (SubDirectoryName).Exists);
		}
		[Test]
		public void AssetSubDirectoryNotExists ()
		{
			Assert.False(ass.GetDirectory("NonExistingSubFolder").Exists);
		}
		[Test]
		public void AssetSubDirectoryNotExistsWithFile ()
		{
			var dir = ass.GetDirectory ("NonExistingSubFolder");
			Assert.IsNotNull(dir);
			Assert.False(dir.Exists);
			Assert.False(dir.FileExists("NonExistingFile.png"));
		}

		[Test]
		public void AssetSubSubDirectoryNotExists ()
		{
			Assert.False(ass.GetDirectory("NonExistingSubFolder/Neither").Exists);
		}
		[Test]
		public void AssetSubSubDirectoryNotExistsWithFile ()
		{
			var dir = ass.GetDirectory ("NonExistingSubFolder/Neither");
			Assert.IsNotNull(dir);
			Assert.False(dir.Exists);
			Assert.False(dir.FileExists("NonExistingFile.png"));
		}

		[Test]
		public void AssetSubDirectoryFileExists ()
		{
			Assert.True(ass.GetDirectory (SubDirectoryName).FileExists(SubDirectoryFile));
		}
		[Test]
		public void AssetSubDirectoryFileNotExists ()
		{
			var dir = ass.GetDirectory (SubDirectoryName);
			Assert.IsNotNull(dir);
			Assert.False(dir.FileExists("NonExistingFile.png"));
		}

		[Test]
		public async void AssetCopyToTempDirectory ()
		{
			string guid = Guid.NewGuid().ToString();
			var tmpPath = FileSystem.Instance.Temp.GetFile(guid);

			var file = ass.GetFile (RootAssetFile);
			Assert.IsNotNull(file);

			var fileCopyRes = await file.CopyToAsync(tmpPath);

			Assert.IsNotNull(fileCopyRes);
			Assert.True(fileCopyRes.Exists);
		}
	}
}

