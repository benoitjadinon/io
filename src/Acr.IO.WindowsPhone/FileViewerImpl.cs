using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;


namespace Acr.IO {

    public class FileViewerImpl : IFileViewer {

        public bool Open(IFile file) {
            return this.OpenFile(file).Result;
        }


        private async Task<bool> OpenFile(IFile file) {
            var store = await StorageFile.GetFileFromPathAsync(file.FullName);
            return await Launcher.LaunchFileAsync(store);
        }
    }
}
