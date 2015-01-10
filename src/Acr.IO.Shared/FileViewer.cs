using System;


namespace Acr.IO {

    public static class FileViewer {

        private static IFileViewer instance;
        private static readonly object syncLock = new object();


        public static IFileViewer Instance {
            get {
                if (instance == null) {
                    lock (syncLock) {
                        if (instance == null) {
#if __PLATFORM__
                            instance = new FileViewerImpl();
#else
                            throw new Exception("Platform implementation not found.  Have you added a nuget reference to your platform project?");
#endif
                        }
                    }
                }
                return instance;
            }
            set { instance = value; }
        }
    }
}
