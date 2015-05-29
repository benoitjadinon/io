using System;


namespace Acr.IO {

    public static class FileSystem {

        private static IFileSystem instance;
        private static readonly object syncLock = new object();


        public static IFileSystem Instance {
            get {
                if (instance == null) {
                    lock (syncLock) {
                        if (instance == null) {
#if __PLATFORM__
                            instance = new FileSystemImpl();
#else
                            throw new NotImplementedException("Platform implementation not found.  Have you added a nuget reference to your platform project?");
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
