using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Areas.Lib.Web;

namespace Areas.CommonMvc.TestWebUI.Controllers
{
    public class AsyncUploadController : BaseController
    {
        //
        // GET: /AsyncUpload/

        public ActionResult UploadaFile()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CaptureFile(HttpPostedFileBase file, string unique)
        {
            if (file != null)
            {
                var sr = new StreamReader(file.InputStream);
                HttpContext.Application["total" + unique] = file.ContentLength;

                var destFile = Server.MapPath("~/uploads/" + Guid.NewGuid().ToString() + file.FileName);
                const int bufferSize = 16384;
                var buffer = new byte[bufferSize];
                var inStream = file.InputStream;
                int bytesCopied = 0;
                UInt64 totalBytes = 0;
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
                            HttpContext.Application["bytes" + unique] = totalBytes;
                            System.Threading.Thread.Sleep(20);
                        }
                    } while (bytesCopied > 0);
                }
            }

            return Json(null);
        }

        public JsonResult GetLatestBytes(string unique)
        {

            if (HttpContext.Application["bytes" + unique] == null)
            {
                return Json(new {bytes = 0, total = 0});
            }

            var bytes_ = Convert.ToInt64(HttpContext.Application["bytes" + unique]);
            var total_ = Convert.ToInt64(HttpContext.Application["total" + unique]);
            if(bytes_ == total_)
            {
                HttpContext.Application["bytes" + unique] = null;
                HttpContext.Application["total" + unique] = null;
            }
            var result = new
                             {
                                 bytes = bytes_,
                                 total = total_
                             };
            

            return Json(result);

        }
    }
}
