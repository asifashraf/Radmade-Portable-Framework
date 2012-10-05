namespace Areas.Lib.UploadProgress
{
    using System;
    using System.Web;
    using System.Web.Script.Serialization;

    using Areas.Lib.UploadProgress.Upload;
    using Areas.Lib.UploadProgress.Upload.AsyncUploadModels;

    public class UploadProgressHandler : IHttpHandler
    {
        public const string DefaultUrl = "~/Telerik.RadUploadProgressHandler.ashx";

        public void ProcessRequest(HttpContext context)
        {
            var jss = new JavaScriptSerializer();

            context.Response.ContentType = "application/json";
            var uploadService = new UploadTrackingsService();

            var track = uploadService.GetTask(context.Request["RadUrid"]);
            if (track.IsNull())
            {
                context.Response.Write(jss.Serialize(new UploadCheckpointResult
                    {
                        InProgress = false,
                        ProgressCounters = false
                    }));
                return;
            }
            else
            {
                context.Response.Write(jss.Serialize(track));
                return;
            }

            var res = jss.Deserialize<UploadCheckpointResult>(track.SerializedData);

            if (res.IsNull())
            {
                context.Response.Write(jss.Serialize(new UploadCheckpointResult
                {
                    InProgress = false,
                    ProgressCounters = false
                }));
                return;
            }

            context.Response.Write(jss.Serialize(res));

            context.Response.End();
            
            return;

            HttpResponse response = context.Response;
            try
            {
                bool flag;
                if (bool.TryParse(context.Request.QueryString["AsyncProgress"], out flag))
                {
                    response.ContentType = "application/json";
                    RadProgressContext.Current.Serialize(response.Output, true);
                }
                else
                {
                    response.ContentType = "text/plain";
                    RadProgressContext.Current.Serialize(response.Output);
                }
            }
            catch (Exception)
            {
                response.Write("Internal server error");
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}