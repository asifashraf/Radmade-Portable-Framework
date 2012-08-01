using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Areas.Lib.HttpModules.FileUploadHelper;
using Areas.Lib.Web;

namespace Area.CommonMvc.Controllers
{
    public class AsyncUploadController : BaseController
    {
        public ActionResult UploadaFile()
        {
            return View();
        }

        [HttpPost]
        public void CaptureFile(HttpPostedFileBase file, string unique)
        {
            if (file != null)
            {

                /*var module = new RadUploadHttpModule();
                module.CaptureWorkerRequest(HttpContext.ApplicationInstance);*/

                var sr = new StreamReader(file.InputStream);
                HttpContext.Cache["total" + unique] = file.ContentLength;

                var destFile = Server.MapPath("~/uploads/" + Guid.NewGuid().ToString() + file.FileName);
                HttpContext.Cache["dest_" + unique] = destFile;
                const int bufferSize = 256;
                var buffer = new byte[bufferSize];
                var inStream = file.InputStream;
                int bytesCopied = 0;
                UInt64 totalBytes = 0;
                inStream.CopyTo(System.IO.File.Create(destFile), bufferSize);
                return;
                using (var outStream = System.IO.File.Open(
                    destFile, FileMode.Create,
                    FileAccess.Write, FileShare.None))
                {
                    do
                    {
                        //must track how many bytes we actually read in
                        bytesCopied = inStream.Read(buffer, 0, bufferSize);
                        if (bytesCopied > 0)
                        {
                            outStream.Write(buffer, 0, bytesCopied);
                            totalBytes += (UInt64) bytesCopied;
                            HttpContext.Cache["bytes" + unique] = totalBytes;
                            //System.Threading.Thread.Sleep(50);
                        }
                    } while (bytesCopied > 0);
                }
            }

            //return Json(null);
        }

        public JsonResult GetLatestBytes(string unique)
        {
            if(HttpContext.Cache["dest_" + unique] != null)
            {
                var destFile = HttpContext.Cache["dest_" + unique].ToString();

                var file = new FileInfo(destFile);

                if(file.Exists)
                {
                    System.Threading.Thread.Sleep(100);
                    var totalBytes = Convert.ToInt64(HttpContext.Cache["total" + unique]);
                    return Json(new
                    {
                        bytes = file.Length,
                        total = totalBytes
                    });
                }
            }


            if (HttpContext.Cache["bytes" + unique] == null)
            {
                return Json(new {bytes = 0, total = 0});
            }
            
            var bytes_ = Convert.ToInt64(HttpContext.Cache["bytes" + unique]);
            var total_ = Convert.ToInt64(HttpContext.Cache["total" + unique]);
            if(bytes_ == total_)
            {
                HttpContext.Cache["bytes" + unique] = null;
                HttpContext.Cache["total" + unique] = null;
            }
            
            var result = new
                             {
                                 bytes = bytes_,
                                 total = total_
                             };
            

            return Json(result);

        }

        public void GetStatus()
        {
            var response = HttpContext.Response;
            try
            {
                bool flag;
                if (bool.TryParse(HttpContext.Request.QueryString["AsyncProgress"], out flag))
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
    }
}
