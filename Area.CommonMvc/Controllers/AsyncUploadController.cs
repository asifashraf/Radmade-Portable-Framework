using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Areas.Lib.Web;

namespace Area.CommonMvc.Controllers
{

    public class AsyncUploadController : BaseController
    {
        public ActionResult UploadaFile()
        {
            return View();
        }

        public JsonResult CreateGuid()
        {
            return this.FormatJson(ResultType.data, "new guid", Guid.NewGuid().ToString());
        }

        [HttpPost]
        public ViewResult CaptureFile(HttpPostedFileBase file, string unique)
        {
            if (file != null)
            {
                var dir = Server.MapPath(@"~/uploads/");
                var di = new DirectoryInfo(dir);
                if(!di.Exists)
                {
                    di.Create();
                }
                var destFile = dir + Guid.NewGuid().ToString() +Path.GetFileName(file.FileName);

                
                file.SaveAs(destFile);
            }

            return this.View();


            /*if (HttpContext.Application["UploadInfos"] == null)
                {
                    HttpContext.Application["UploadInfos"] = new List<UploadTimer>();
                }

                var uploadInfos = HttpContext.Application["UploadInfos"] as List<UploadTimer>;*/

            //var serviceHolder = new ServiceHolder<UploadTrackingDataContext>();



            /*const int bufferSize = 512;
                var inStream = file.InputStream;



                inStream.CopyTo(System.IO.File.Create(destFile), bufferSize);
                inStream.Close();*/

            /*using (var outStream = System.IO.File.Open(
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
                }*/


        }

            //return Json(null);
        }
    }

