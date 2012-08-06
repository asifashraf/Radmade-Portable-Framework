namespace Areas.Lib.UploadProgress
{
    using System;
    using System.Web;

    public class UploadProgressHandler : IHttpHandler
    {
        public const string DefaultUrl = "~/Telerik.RadUploadProgressHandler.ashx";

        public void ProcessRequest(HttpContext context)
        {
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