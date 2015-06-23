using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITests
{
	[TestFixture (Platform.Android)]
	[TestFixture (Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;

		public Tests (Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest ()
		{
			app = AppInitializer.StartApp (platform);
			app.Repl();
		}

		[Test]
		public void AssetsFolderExists () {
			
		}

		[Test]
		public void WebViewOpensTheFile ()
		{
			//app.Screenshot ("First screen.");
			//var but = app.Query().First(p => p.Text == "Open Uri ");
			var q = app.Query(p=>p.Button("Open Uri "));
			//Assert.AreEqual(q.Count, 1);
			app.Tap(p => p.Button("Open Uri "));
			app.WaitForElement(p => p.WebView());

		}
	}
}