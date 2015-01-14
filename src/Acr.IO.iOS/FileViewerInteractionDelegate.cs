using System;
using UIKit;


namespace Acr.IO {

    public class FileViewerInterationDelegate : UIDocumentInteractionControllerDelegate {
        private readonly UIViewController controller;
        private readonly UIView view;


        public FileViewerInterationDelegate() {
            this.controller = Utils.GetTopViewController();
            this.view = Utils.GetTopView();
        }


        public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller) {
            return this.controller;
        }


        public override UIView ViewForPreview(UIDocumentInteractionController controller) {
            return this.view;
        }
    }
}