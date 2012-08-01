namespace Areas.Lib.HttpModules.FileUploadHelper
{

    internal class RadAsyncUploadContext : RadUploadContext
    {
        private int uploadsInProgress;
        private readonly object uploadsLock;

        internal RadAsyncUploadContext(int requestLength, RequestStateStore stateStore)
            : base(requestLength, stateStore)
        {
            this.uploadsLock = new object();
        }

        public int UploadsInProgress
        {
            get
            {
                lock (this.uploadsLock)
                {
                    return this.uploadsInProgress;
                }
            }
            set
            {
                lock (this.uploadsLock)
                {
                    this.uploadsInProgress = value;
                }
            }
        }
    }
}