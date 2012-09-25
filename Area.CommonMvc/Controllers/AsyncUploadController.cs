using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Areas.Lib.Web;
using Areas.Lib.UploadProgress.Upload.AsyncUploadModels;

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
            //Update file full path in the DB
            var trackerService = new UploadTrackingsService();
            if (file != null)
            {
                var dir = Server.MapPath(@"~/uploads/");
                var di = new DirectoryInfo(dir);
                trackerService.Log(Request["RadUrid"], "AsyncController.CaptureFile", "directoryInfo.FullPath", di.FullName);
                if (!di.Exists)
                {
                    di.Create();
                    trackerService.Log(Request["RadUrid"], "AsyncController.CaptureFile", "!di.Exists", "Created directory");
                }            

                var destFile = dir + Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);

                trackerService.UpdateFileFullPath(Request["RadUrid"], destFile);

                trackerService.Log(Request["RadUrid"], "AsyncController.CaptureFile", "trackerService.UpdateFileFullPath(Request[\"RadUrid\"], destFile)", destFile);

                try
                {
                    trackerService.Log(Request["RadUrid"], "AsyncController.CaptureFile", "Starting file.SaveAs(destFile)", "");
                    file.SaveAs(destFile);
                    trackerService.Log(Request["RadUrid"], "AsyncController.CaptureFile", "file.SaveAs(destFile) successful", "");
                }
                catch(Exception error)
                {
                    trackerService.MarkAsComplete(Request["RadUrid"], file.ContentLength, error.Message);
                    trackerService.Log(Request["RadUrid"], "AsyncController.CaptureFile", "file.SaveAs(destFile) Error occured", error.ToString());
                }
                finally
                {
                    trackerService.MarkAsComplete(Request["RadUrid"], file.ContentLength );
                    trackerService.Log(Request["RadUrid"], "AsyncController.CaptureFile", "finally block marking as complete",
                        new { file.ContentLength });
                }
            }

            return View();
        }

        public JsonResult GetFileFullName(string clientId)
        {
            //Update file full path in the DB
            var trackerService = new UploadTrackingsService();
            var fileName = string.Empty;

            while (fileName.IsNullOrEmpty())
            {
                trackerService.Log(Request["RadUrid"], "AsyncController.GetFileFullName", "fileName.IsNullOrEmpty()", "");
                System.Threading.Thread.Sleep(500);
                var track = trackerService.GetTask(clientId);
                fileName = track.FileFullPath;
                if(fileName.IsNotNullOrEmpty())
                {
                    trackerService.Log(Request["RadUrid"], "AsyncController.GetFileFullName", "fileName.IsNotNullOrEmpty()", fileName);
                    return this.FormatJson(ResultType.data, "FileName", track, string.Empty);   
                }
            }
            return null;
        }

            //return Json(null);
        }
    }

