using System;
using System.Linq;
using System.Reflection;
using Acr.IO;
using Xamarin.Forms;


namespace Samples {

    public class App : Application {

        public App() {

            this.MainPage = new ContentPage {
                Content = new StackLayout {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Button {
                            Text = "Open Test PDF",
                            Command = new Command(async () => {
                                var file = FileSystem.Instance.Public.GetFile("temp.pdf");
                                file.DeleteIfExists();

                                var assembly = Assembly.Load(new AssemblyName("Samples"));
                                using (var stream = assembly.GetManifestResourceStream("test.pdf"))
                                    using (var fs = file.OpenWrite())
                                        stream.CopyTo(fs);

                                if (!FileViewer.Instance.Open(file))
                                    await this.MainPage.DisplayAlert("ERROR", "Could not open file " + file.FullName, "OK");
                            })
                        },
                        new Label {
                            Text = "App Data: " + FileSystem.Instance.AppData.FullName,
                            FontSize = 10,
                        },
                        new Label {
							Text = "Cache Data: " + FileSystem.Instance.Cache.FullName,
                            FontSize = 10,
                        },
                        new Label {
							Text = "Public: " + FileSystem.Instance.Public.FullName,
                            FontSize = 10,
                        },
						new Label {
							Text = "Temp: " + FileSystem.Instance.Temp.FullName,
                            FontSize = 10,
                        },
						new Label {
							Text = "Assets/Resources: " + FileSystem.Instance.Assets.Name,
                            FontSize = 10,
                        },
						new Label {
							Text = "Assets/Resources Default.png exists ? " + (FileSystem.Instance.Assets.FileExists("Default.png") ? "yes":"no"),
                            FontSize = 10,
                        },
						new Label {
							Text = "Assets/Resources Get Default.png " + FileSystem.Instance.Assets.GetFile("Default.png")?.OpenRead(),
                            FontSize = 10,
                        },
						new Label {
							Text = "Assets/Resources Default.png Length " + FileSystem.Instance.Assets.GetFile("Default.png")?.Length,
                            FontSize = 10,
                        },
						new Label {
							Text = "Assets/Resources sub-directories " + FileSystem.Instance.Assets.Directories?.Count(),
                            FontSize = 10,
                        },
						new Label {
							Text = "Assets/Resources sub-directory " + (FileSystem.Instance.Assets.GetSubDirectory("webkit").Exists ? "yes" : "no"),
                            FontSize = 10,
                        },
						new Label {
							Text = "Assets/Resources files " + FileSystem.Instance.Assets.Files?.Count(),
                            FontSize = 10,
                        },
					}
                }
            };
        }
    }
}
