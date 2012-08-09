namespace Areas.Lib.UploadProgress.Upload.AsyncUploadModels
{
    using System;

    using Areas.Lib.LinqToSql;

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

            this.SaveChanges();

            return task;
        }
    }
}
