using System;
using Acr.IO;
using Foundation;
using UIKit;
using Xamarin.Forms;


namespace Samples.iOS {

    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate {


        public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
            Forms.Init();
            FileSystem.Init();
            FileViewer.Init();

            this.LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
    }
}
