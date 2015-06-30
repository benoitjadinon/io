using System;
using System.Linq;
using System.Reflection;
using Acr.IO;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Collections;


namespace Samples {

    public class App : Application {

		const string AssetDefaultPNG = "Default.png";
		const string AssetSubFolderIconPNG = "Icon.png";

		const string AssetSubFolder = "SubFolder";

        public App() {
			Button uriButton;
			Button copyButton;

			var assetsDirectories = FileSystem.Instance.Assets.Directories;
            var assetFilesInRoot = FileSystem.Instance.Assets.Files;
			var fileDefaultPNG = FileSystem.Instance.Assets.GetFile(AssetDefaultPNG);
			var subFolder = FileSystem.Instance.Assets.GetDirectory (AssetSubFolder);
			var fileSubIconPNG = subFolder.GetFile (AssetSubFolderIconPNG);

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
								Text = "Assets/Resources Default.png exists ? " + (FileSystem.Instance.Assets.FileExists (AssetDefaultPNG).PrettyPrint()),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources Get Default.png Stream : " + (fileDefaultPNG != null ? fileDefaultPNG.OpenRead().ToString() : "no stream"),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources Default.png Length : " + (fileDefaultPNG != null) ? fileDefaultPNG.Length : "Empty",
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources sub-directories : " + assetsDirectories.PrettyPrint() + " : " + assetsDirectories.Select(p=>p.Name).ToArray().PrettyPrint(),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources sub-directory 'SubFolder' exists ? : " + (subFolder.Exists.PrettyPrint()),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources sub-directory files : " + subFolder.Files.PrettyPrint(),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources sub-directory file Icon.Png ? " + (fileSubIconPNG.Exists.PrettyPrint()),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources files in folder : " + assetFilesInRoot.PrettyPrint(),
	                            FontSize = 10,
	                        },
							new Label {
								Text = "Assets/Resources uri file Icon.Png ? " + (fileSubIconPNG.Uri),
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

			uriButton.Clicked += (object sender, EventArgs e) => OpenWebView(fileSubIconPNG.Uri);
			copyButton.Clicked += async (object sender, EventArgs e) => {
				copyButton.IsEnabled = false;
				var targetFile = FileSystem.Instance.Public.GetFile("acr.io.asset.copy.test.TOREMOVE.png");
				await fileSubIconPNG.CopyToAsync (targetFile.FullName);
				copyButton.Text += " Copy " + targetFile.Exists.PrettyPrint();
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

    public static class ArrayExtensions 
    {
		public static string PrettyPrint(this object[] array, string sep = ",")
		{
			string ret = "";
			foreach (var item in array ?? new string[0]) {
				ret += item + sep;
			}
			return array.Any() ? ret.Substring(0, ret.Length-1) : ret;
        }
    }

    public static class BooleanExtensions 
    {
		public static string PrettyPrint(this bool boo)
		{
			return boo ? "yes" : "no";
        }
        public static string PrettyPrint (this bool? boo)
		{
			if (!boo.HasValue) 
				return "null";

			return boo.Value.PrettyPrint();
		}
    }

    public static class IEnumerableExtensions
    {
    	public static string PrettyPrint(this IEnumerable list){
    		if (list == null)
    			return "null";

    		return list.Count().ToString();
    	}
    }
}
