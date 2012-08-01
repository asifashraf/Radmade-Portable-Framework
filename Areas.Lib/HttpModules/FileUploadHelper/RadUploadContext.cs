using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Areas.Lib.HttpModules.FileUploadHelper
{
    public class RadUploadContext
    {
        private DateTime _startTime = DateTime.Now;
        private UploadedFileCollection _uploadedFiles;
        internal static readonly string UNIQUE_REQUEST_QUERY_IDENTIFIER = "RadUrid";

        internal RadUploadContext(int requestLength, RequestStateStore stateStore)
        {
            this.RequestLength = requestLength;
            this.StateStore = stateStore;
        }

        private FileHeaderInfo FindLastUploadedFile()
        {
            for (int i = this.StateStore.Fields.Count - 1; i >= 0; i--)
            {
                RequestField field = this.StateStore.Fields[i];
                if (field.Header is FileHeaderInfo)
                {
                    return (FileHeaderInfo)field.Header;
                }
            }
            return null;
        }

        private string FormatBytes(int bytes)
        {
            int megaByte = 0x400;
            int num2 = (int)Math.Pow((double)megaByte, 2.0);
            int num3 = (int)Math.Pow((double)megaByte, 3.0);
            decimal num4 = 0.8M;
            if (bytes > (num4 * num3))
            {
                return FormatBytes("{0}GB", bytes, num3);
            }
            if (bytes > (num4 * num2))
            {
                return FormatBytes("{0}MB", bytes, num2);
            }
            if (bytes > (num4 * megaByte))
            {
                return FormatBytes("{0}kB", bytes, megaByte);
            }
            return string.Format("{0}B", bytes);
        }

        private static string FormatBytes(string formatString, int bytes, int megaByte)
        {
            return string.Format(formatString, Math.Round((decimal)(bytes / megaByte), 2).ToString("0.00", CultureInfo.InvariantCulture));
        }

        private int GetCompleteFileCount()
        {
            int num = 0;
            for (int i = 0; i < this.StateStore.Fields.Count; i++)
            {
                RequestField field = this.StateStore.Fields[i];
                if (this.isFileField(field))
                {
                    num++;
                }
            }
            return num;
        }

        public static RadUploadContext GetCurrent(HttpContext context)
        {
            return (context.Application["RadUploadContext" + GetUploadUniqueIdentifier(context)] as RadUploadContext);
        }

        private int GetEstimatedTime()
        {
            decimal speed = this.GetSpeed();
            if ((this.ElapsedMilliseconds != 0) && (speed > 0M))
            {
                decimal num2 = (this.RequestLength / speed) * 1000M;
                return (int)Math.Round((decimal)(num2 - this.ElapsedMilliseconds));
            }
            return 0x7fffffff;
        }

        private string GetFormattedSpeed()
        {
            return string.Format("{0}/s", this.FormatBytes(Convert.ToInt32(this.GetSpeed())));
        }

        internal ProgressData GetProgressData()
        {
            RadUploadProgressData progressData = new RadUploadProgressData();
            if (this.StateStore != null)
            {
                this.PopulateProgressData(progressData);
            }
            return progressData;
        }

        private decimal GetSpeed()
        {
            if (this.ElapsedMilliseconds == 0)
            {
                return 0M;
            }
            return ((this.StateStore.CurrentRequestBytesCount / this.ElapsedMilliseconds) * 0x3e8);
        }

        internal static string GetUploadUniqueIdentifier(HttpContext context)
        {
            return context.Request.QueryString[UNIQUE_REQUEST_QUERY_IDENTIFIER];
        }

        private bool isFileField(RequestField field)
        {
            return (((field.Header is FileHeaderInfo) && field.Complete) && !string.IsNullOrEmpty(((FileHeaderInfo)field.Header).FileName));
        }

        private void PopulateProgressData(RadUploadProgressData progressData)
        {
            FileHeaderInfo info = this.FindLastUploadedFile();
            progressData.CurrentOperationText = string.Empty;
            if (info != null)
            {
                progressData.CurrentOperationText = info.FileName;
            }
            progressData.PrimaryTotal = this.FormatBytes(this.RequestLength);
            progressData.PrimaryValue = this.FormatBytes(this.StateStore.CurrentRequestBytesCount);
            progressData.PrimaryPercent = (int)Math.Round((decimal)((this.StateStore.CurrentRequestBytesCount / this.RequestLength) * 100M));
            progressData.SecondaryValue = this.GetCompleteFileCount();
            progressData.Speed = this.GetFormattedSpeed();
            progressData.TimeElapsed = this.ElapsedMilliseconds;
            progressData.TimeEstimated = this.GetEstimatedTime();
            progressData.RequestLength = this.RequestLength;
            progressData.CompleteBytes = this.StateStore.CurrentRequestBytesCount;
            progressData.OperationComplete = this.UploadComplete;
        }

        internal static void RemoveUploadContext(HttpContext context)
        {
            context.Application.Remove("RadUploadContext" + GetUploadUniqueIdentifier(context));
        }

        internal static void SetUploadContext(HttpContext context, RadUploadContext uploadContext)
        {
            context.Application["RadUploadContext" + GetUploadUniqueIdentifier(context)] = uploadContext;
        }

        public static RadUploadContext Current
        {
            get
            {
                return (HttpContext.Current.Application["RadUploadContext" + GetUploadUniqueIdentifier(HttpContext.Current)] as RadUploadContext);
            }
        }

        internal int ElapsedMilliseconds
        {
            get
            {
                return (int)DateTime.Now.Subtract(this._startTime).TotalMilliseconds;
            }
        }

        internal bool IsUploadInProgress
        {
            get
            {
                return ((this.StateStore == null) || (this.StateStore.Fields.Count > 0));
            }
        }

        internal int RequestLength { get; set; }

        private RequestStateStore StateStore { get; set; }

        internal bool UploadComplete
        {
            get
            {
                if (this.StateStore == null)
                {
                    return false;
                }
                return this.StateStore.UploadComplete;
            }
        }

        internal int UploadedBytes
        {
            get
            {
                if (this.StateStore != null)
                {
                    return this.StateStore.CurrentRequestBytesCount;
                }
                return 0;
            }
        }

        [Obsolete("The HttpContext.Current.UploadedFiles collection is now deprecated. Use the Request.Files collection instead", false)]
        public UploadedFileCollection UploadedFiles
        {
            get
            {
                if (this._uploadedFiles == null)
                {
                    if ((HttpContext.Current == null) || (HttpContext.Current.Request == null))
                    {
                        return new UploadedFileCollection();
                    }
                    this._uploadedFiles = new UploadedFileCollection();
                    HttpRequest request = HttpContext.Current.Request;
                    foreach (string str in request.Files)
                    {
                        HttpPostedFile file = request.Files[str];
                        if (((file != null) && !string.IsNullOrEmpty(file.FileName)) && (file.InputStream != null))
                        {
                            this._uploadedFiles.Add(UploadedFile.FromHttpPostedFile(str, request.Files[str]));
                        }
                    }
                }
                return this._uploadedFiles;
            }
        }

        internal int UploadedFilesCount
        {
            get
            {
                if (this.StateStore == null)
                {
                    return 0;
                }
                int num = 0;
                foreach (RequestField field in this.StateStore.Fields)
                {
                    if (field.Header is FileHeaderInfo)
                    {
                        num++;
                    }
                }
                return num;
            }
        }
    }
}