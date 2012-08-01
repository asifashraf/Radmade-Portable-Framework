using System;
using System.Reflection;
using System.Web;

namespace Areas.Lib.HttpModules.FileUploadHelper
{
    public class RadUploadHttpModule : IHttpModule
    {
        private HttpApplication _application;

        protected virtual void CaptureWorkerRequest(object sender, EventArgs e)
        {
            _application = sender as HttpApplication;
            Context = Application.Context;
            if (IsUploadRequest(Application))
            {
                var workerRequestField = GetWorkerRequestField();
                if ((workerRequestField != null))
                {
                    var workerRequest = workerRequestField.GetValue(Context.Request) as HttpWorkerRequest;
                    if (workerRequest != null)
                    {
                        var progressWorker = GetProgressWorker(workerRequest);
                        UpdateUploadContext(progressWorker);
                        workerRequestField.SetValue(Context.Request, progressWorker);
                    }
                }
            }
        }

        public void CaptureWorkerRequest(HttpApplication application)
        {
            Application = application;
            Context = application.Context;
            if (IsUploadRequest(Application))
            {
                var workerRequestField = GetWorkerRequestField();
                if ((workerRequestField != null))
                {
                    var workerRequest = workerRequestField.GetValue(Context.Request) as HttpWorkerRequest;
                    if (workerRequest != null)
                    {
                        var progressWorker = GetProgressWorker(workerRequest);
                        UpdateUploadContext(progressWorker);
                        workerRequestField.SetValue(Context.Request, progressWorker);
                    }
                }
            }
        }

        private RadUploadContext CreateContext(ProgressWorkerRequest progressWorker)
        {/*
            if (this.IsAsyncUploadRequest)
            {
                RadAsyncUploadContext context;
                return new RadAsyncUploadContext(this.Context.Request.ContentLength, progressWorker.RequestStateStore) { UploadsInProgress = context.UploadsInProgress + 1 };
            }*/
            return new RadUploadContext(this.Context.Request.ContentLength, progressWorker.RequestStateStore);
        }

        public void Dispose()
        {
        }

        private ProgressWorkerRequest GetProgressWorker(HttpWorkerRequest workerRequest)
        {
            if (this.IsAsyncUploadRequest)
            {
                return new AsyncProgressWorkerRequest(workerRequest, this.Context.Request);
            }
            return new ProgressWorkerRequest(workerRequest, this.Context.Request);
        }

        private FieldInfo GetWorkerRequestField()
        {
            FieldInfo field = this.Context.Request.GetType().GetField("_wr", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
            {
                field = this.Context.Request.GetType().GetField("worker_request", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return field;
        }

        public virtual void Init(HttpApplication app)
        {
            app.PreRequestHandlerExecute += CaptureWorkerRequest;
            app.PostRequestHandlerExecute += ReleaseWorkerRequest;
            app.Error += ReleaseWorkerRequest;
        }

        private bool IsUploadRequest(HttpApplication application)
        {
            return (((application.Request != null) && (application.Request.ContentType != null)) && application.Request.ContentType.ToLower().StartsWith("multipart/form-data"));
        }

        private void ReleaseContexts()
        {
            if (this.IsAsyncUploadRequest)
            {
                RadAsyncUploadContext current = RadUploadContext.Current as RadAsyncUploadContext;
                if (current == null)
                {
                    return;
                }
                current.UploadsInProgress--;
                if (current.UploadsInProgress > 0)
                {
                    return;
                }
            }
            RadProgressContext.RemoveProgressContext(this.Context);
            RadUploadContext.RemoveUploadContext(this.Context);
        }

        protected virtual void ReleaseWorkerRequest(object sender, EventArgs e)
        {
            if (this.Application == null)
            {
                this._application = sender as HttpApplication;
            }
            if (this.IsUploadRequest(this.Application))
            {
                FieldInfo workerRequestField = this.GetWorkerRequestField();
                this.ReleaseContexts();
                if ((workerRequestField != null))
                {
                    ProgressWorkerRequest request = workerRequestField.GetValue(this.Context.Request) as ProgressWorkerRequest;
                    if (request != null)
                    {
                        workerRequestField.SetValue(this.Context.Request, request._originalWorkerRequest);
                    }
                }
            }
        }

        private void UpdateUploadContext(ProgressWorkerRequest progressWorker)
        {
            if (RadUploadContext.GetCurrent(this.Context) == null)
            {
                RadUploadContext.SetUploadContext(this.Context, this.CreateContext(progressWorker));
            }
            else if (this.IsAsyncUploadRequest)
            {
                RadAsyncUploadContext current = RadUploadContext.Current as RadAsyncUploadContext;
                if (current != null)
                {
                    current.RequestLength += this.Context.Request.ContentLength;
                    current.UploadsInProgress++;
                }
            }
        }

        private HttpApplication Application
        {
            get
            {
                return this._application;
            }
            set { this._application = value; }
        }

        public HttpContext Context { get; set; }

        private bool IsAsyncUploadRequest
        {
            get
            {
                return false;
            }
        }

        public static bool IsRegistered
        {
            get
            {
                if (!SecurityHelper.IsPermissionGranted(new AspNetHostingPermission(AspNetHostingPermissionLevel.High)))
                {
                    return true;
                }
                HttpModuleCollection modules = HttpContext.Current.ApplicationInstance.Modules;
                foreach (string str in modules.AllKeys)
                {
                    if (modules[str] is RadUploadHttpModule)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}