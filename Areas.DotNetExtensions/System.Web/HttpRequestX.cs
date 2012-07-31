using System.Web;
	public static class HttpRequestX
	{
        public static string FindValue(this HttpContextBase context,string key)
        {
            if (context.IsNotNull())
            {
                if (context.Request.Headers[key].IsNotNullOrEmpty())
                    return context.Request.Headers[key];
                if (context.Request.Form[key].IsNotNullOrEmpty())
                    return context.Request.Form[key];
                if (context.Request.QueryString[key].IsNotNullOrEmpty())
                    return context.Request.QueryString[key];
            }
            else
            {
                var ctx = HttpContext.Current;
                if (ctx.Request.Headers[key].IsNotNullOrEmpty())
                    return ctx.Request.Headers[key];
                if (ctx.Request.Form[key].IsNotNullOrEmpty())
                    return ctx.Request.Form[key];
                if (ctx.Request.QueryString[key].IsNotNullOrEmpty())
                    return ctx.Request.QueryString[key];
            }

            return null;
        }

        public static string FindValue(this HttpContext context,string key)
        {
            if (context.IsNull())
            {
                context = HttpContext.Current;
            }
            if (context.Request.Headers[key].IsNotNullOrEmpty())
                return context.Request.Headers[key];
            if (context.Request.Form[key].IsNotNullOrEmpty())
                return context.Request.Form[key];
            if (context.Request.QueryString[key].IsNotNullOrEmpty())
                return context.Request.QueryString[key];
            return null;
        }
    }
