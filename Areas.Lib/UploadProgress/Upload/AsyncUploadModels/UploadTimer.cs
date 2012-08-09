namespace Areas.Lib.UploadProgress.Upload.AsyncUploadModels
{
    using System;
    using System.Web;

    public class UploadTimer : System.Timers.Timer
    {
        public long TaskId { get; set; }

        public string UniqueId { get; set; }

        public DateTime StartDate { get; set; }

        public HttpContext HttpContext { get; set; }

        internal ProgressWorkerRequest ProgressWorkerRequest { get; set; }

        public RadUploadContext RadUploadContext { get; set; }
    }
}
