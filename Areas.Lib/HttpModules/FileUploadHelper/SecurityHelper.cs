using System.Security;

namespace Areas.Lib.HttpModules.FileUploadHelper
{
    internal static class SecurityHelper
    {
        public static bool IsPermissionGranted(IPermission permission)
        {
            return SecurityManager.IsGranted(permission);
        }
    }
}