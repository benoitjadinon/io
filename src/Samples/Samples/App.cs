﻿using System;
using System.Linq;
using System.Reflection;
using Acr.IO;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;


namespace Samples {

    public class App : Application {

        public App() {
			Button uriButton;
			Button copyButton;

            this.MainPage = new NavigationPage();
            (this.MainPage as NavigationPage).PushAsync(
            new ContentPage {
                Content = new ScrollView {
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
								Text = "Assets/Resources: " + FileSystem.Instance.Assets.FullName,
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
								Text = "Assets/Resources Get Default.png : " + FileSystem.Instance.Assets.GetFile("Default.png")?.OpenRead(),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources Default.png Length : " + FileSystem.Instance.Assets.GetFile("Default.png")?.Length,
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources sub-directories : " + FileSystem.Instance.Assets.Directories?.Count() + " : " + FileSystem.Instance.Assets.Directories?.Select(p=>p.Name).ToArray().PrintArray(),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources sub-directory 'SubFolder' exists ? : " + (FileSystem.Instance.Assets.GetDirectory("SubFolder").Exists ? "yes" : "no"),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources sub-directory files : " + FileSystem.Instance.Assets.GetDirectory("SubFolder").Files?.Count(),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources sub-directory file Icon.Png ? " + (FileSystem.Instance.Assets.GetDirectory("SubFolder").GetFile("Icon.png").Exists ? "yes" : "no"),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources files in folder : " + FileSystem.Instance.Assets.Files?.Count(),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources uri file Icon.Png ? " + (FileSystem.Instance.Assets.GetDirectory("SubFolder").GetFile("Icon.png").Uri),
	                            FontSize = 10,
	                        },
							(uriButton = new Button {
								Text = "Open Uri ",
	                            FontSize = 10,
	                        }),
							(copyButton = new Button {
								Text = "Copy File from Assets to Public (async)",
	                            FontSize = 10,
	                        }),
						}
					}
                }
            });

			uriButton.Clicked += (object sender, EventArgs e) => OpenWebView(FileSystem.Instance.Assets.GetDirectory("SubFolder").GetFile("Icon.png").Uri);
			copyButton.Clicked += async (object sender, EventArgs e) => {
				copyButton.IsEnabled = false;
				var targetFile = FileSystem.Instance.Public.GetFile("acr.io.asset.copy.test.TOREMOVE.png");
				await FileSystem.Instance.Assets.GetDirectory ("SubFolder").GetFile ("Icon.png").CopyToAsync (targetFile.FullName);
				copyButton.Text += " Copy " + (targetFile.Exists?"OK":"FAIL");
				await Task.Delay(1500);
				copyButton.IsEnabled = true;
				OpenWebView(targetFile.Uri);
				await Task.Delay(1500);
				targetFile.DeleteIfExists();
			};
        }

		protected void OpenWebView (Uri uri)
		{
			(MainPage as NavigationPage).PushAsync(new ContentPage(){
				Content = new WebView{
					Source = uri,
				}
			});
		}
    }

    public static class ArrayExtensions {
		public static string PrintArray(this object[] array, string sep = ","){
			string ret = "";
			foreach (var item in array ?? new string[0]) {
				ret += item + sep;
			}
			return array.Any() ? ret.Substring(0, ret.Length-1) : ret;
        }
    }
}
