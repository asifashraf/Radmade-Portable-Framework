namespace Areas.Lib.HttpModules.FileUploadHelper
{
    using System.Web;

    internal class AsyncProgressWorkerRequest : ProgressWorkerRequest
    {
        public AsyncProgressWorkerRequest(HttpWorkerRequest wr, HttpRequest request)
            : base(wr, request)
        {
        }

        protected override void UpdateProgress(byte[] buffer, int validBytes)
        {
            base.RequestStateStore.UpdateCurrentRequestBytesCount(validBytes);
        }
    }

}