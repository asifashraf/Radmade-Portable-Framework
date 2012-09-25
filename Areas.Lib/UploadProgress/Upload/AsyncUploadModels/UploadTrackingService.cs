namespace Areas.Lib.UploadProgress.Upload.AsyncUploadModels
{
    using System;
    using System.Globalization;

    using Areas.Lib.LinqToSql;
    using System.Web.Script.Serialization;

    public class UploadTrackingsService : AbstractService<UploadTrackingDataContext>
    {
        public UploadTracking CreateTask(string uid, string serializedData)
        {
            var tracking = new UploadTracking
                {
                    ClientId = uid,
                    StartTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    SerializedData = serializedData
                };

            return this.Create(tracking);
        }

        public UploadTracking GetTask(long taskId)
        {
            return this.Get<UploadTracking>(ut => ut.TaskId == taskId);
        }

        public UploadTracking GetTask(string uniqueId)
        {
            return this.Get<UploadTracking>(ut => ut.ClientId.ToLower() == uniqueId);
        }

        public UploadTracking UpdateTaskData(long taskId, string serializedData, UploadCheckpointResult checkpoint = null)
        {
            var task = this.Get<UploadTracking>(ut => ut.TaskId == taskId);
            task.SerializedData = serializedData;
            task.UpdateTime = DateTime.Now;
            
            //tell when progress started
            if(task.Done.IsNull() && checkpoint.Done > 0)
            {
                task.StartedProgressingAt = DateTime.Now;
            }
            if(task.Done.IsNotNull())
            {
                if(task.Done.Value == 0 && checkpoint.Done > 0)
                {
                    task.StartedProgressingAt = DateTime.Now;
                }
            }

            //tell when progress stopped
            if(task.StartedProgressingAt.IsNotNull() && checkpoint.InProgress == false)
            {
                task.StoppedProgressingAt = DateTime.Now;
                task.Completed = true;
            }
            else
            {
                if (checkpoint.IsNotNull())
                {
                    checkpoint.CopyMatchProperties(task);
                    task.Total = checkpoint.Total;
                    task.Done = checkpoint.Done;
                }
            }

            try
            {
                this.SaveChanges();
            }
            catch
            {
                
            }
            

            return task;
        }

        public UploadTracking UpdateFileFullPath(string clientId, string fileFullPath)
        {
            var task = this.Get<UploadTracking>(ut => ut.ClientId.ToLower() == clientId.ToLower());

            if (task.IsNotNull())
            {
                task.FileFullPath = fileFullPath;
            }

            this.SaveChanges();

            return task;
        }

        public UploadTracking MarkAsComplete(string clientId, long contentLength, string errorText = null)
        {
            var task = this.Get<UploadTracking>(ut => ut.ClientId.ToLower() == clientId.ToLower());

            if (task.IsNotNull())
            {
                task.Completed = true;
                var mbs = (contentLength / 1024) / 1024;
                task.PrimaryTotal = string.Format("{0}MB", mbs);
                task.PrimaryValue = string.Format("{0}MB", mbs);
                task.Total = mbs;
                task.Done = mbs;
                task.ErrorText = errorText;
            }

            this.SaveChanges();

            return task;
        }

        public void Log(string clientId, string mainFunction, string title, object dataDump, string extra1, string extra2, string extra3)
        {
            var jss = new JavaScriptSerializer();

            var log = new UploadTrackingLog
            {
                ClientId = clientId, DataDump = jss.Serialize(dataDump), Extra1 = extra1, Extra2 = extra2, Extra3 = extra3,
                LogDate = DateTime.Now, MainFunction = mainFunction, Title = title
            };

            DataContext.UploadTrackingLogs.InsertOnSubmit(log);

            DataContext.SubmitChanges();
        }

        public void Log(string clientId, string mainFunction, string title, object dataDump)
        {
            Log(clientId, mainFunction, title, dataDump, "", "", "");
        }
    }
}
