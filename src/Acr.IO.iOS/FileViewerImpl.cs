using System;
using UIKit;


namespace Acr.IO {

	public class FileViewerImpl : IFileViewer {

		public bool Open(IFile file) {
			var opened = false;

			if (url.IsFileUrl && UIApplication.SharedApplication.CanOpenUrl(url)) {
				using (var controller = UIDocumentInteractionController.FromUrl(url)) {
					UIApplication.SharedApplication.InvokeOnMainThread(() => {
						controller.Delegate = new FileViewerInterationDelegate(); 
						opened = controller.PresentPreview(true);
					});
				}
			}
			return opened;
		}
	}
}