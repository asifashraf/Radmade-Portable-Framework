using System.Web; // For the reference to HttpContext
using System.Web.Management;


  public class WebAuditEventX : WebAuditEvent
  {
    private string userID;
    private string authType;
    private bool isAuthenticated;

    public WebAuditEventX(string msg, object eventSource, int eventCode)
      : base(msg, eventSource, eventCode)
    {
      // Obtain the HTTP Context and store authentication details
      userID = HttpContext.Current.User.Identity.Name;
      authType = HttpContext.Current.User.Identity.AuthenticationType;
      isAuthenticated = HttpContext.Current.User.Identity.IsAuthenticated;
    }

    public WebAuditEventX(string msg, object eventSource, int eventCode,
                           int eventDetailCode)
      : base(msg, eventSource, eventCode, eventDetailCode)
    {
      // Obtain the HTTP Context and store authentication details
      userID = HttpContext.Current.User.Identity.Name;
      authType = HttpContext.Current.User.Identity.AuthenticationType;
      isAuthenticated = HttpContext.Current.User.Identity.IsAuthenticated;
    }


}