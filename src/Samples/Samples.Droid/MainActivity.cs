using System;
using Acr.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;


namespace Samples.Droid {

    [Activity(Label = "Samples", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity {

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            FileSystem.Init();
            FileViewer.Init();
            this.LoadApplication(new App());
        }
    }
}

