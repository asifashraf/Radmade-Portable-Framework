using System.Linq;
using System.Web;


    public static  class HttpContextX
    {
        public static string DomainHttp(this HttpContext context)
        {
            var full = HttpContext.Current.Request.Url.ToString();
            var afterHttp = full.Substring(7);
            afterHttp = afterHttp.Contains('/') ? afterHttp.Substring(0, afterHttp.IndexOf('/')) : afterHttp;
            return @"http://" + afterHttp;

        }
		public static void TraceRequest(this HttpContext context)
		{
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			var form = request.Form;

			response.Write("<html>");
			response.Write("<body>");

			#region form
			response.Write("<h1>");
			response.Write("FORM");
			response.Write("</h1>");
			response.Write("<div>");
			foreach (var key in form.AllKeys)
			{
				response.Write("<br />");
				response.Write(key + ":" + form[key]);
			}
			response.Write("</div>");
			#endregion

			#region files
			response.Write("<h1>");
			response.Write("FILES");
			response.Write("</h1>");
			response.Write("<div>");
			foreach (HttpPostedFile file in request.Files)
			{
				response.Write("<br />");
				response.Write("File Name" + ":" + file.FileName);
				response.Write("<br />");
				response.Write("File Size" + ":" + file.ContentLength);
				response.Write("<br />");
				response.Write("Content Type" + ":" + file.ContentType);
			}
			response.Write("</div>");
			#endregion

			response.Write("</body>");
			response.Write("</html>");
			response.End();
		}
}
