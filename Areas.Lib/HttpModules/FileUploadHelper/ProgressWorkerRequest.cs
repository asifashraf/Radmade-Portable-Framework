
namespace Areas.Lib.HttpModules.FileUploadHelper
{
    using System;
    using System.Web;

    internal class ProgressWorkerRequest : HttpWorkerRequest
    {
        private byte[] _boundary;
        public HttpWorkerRequest _originalWorkerRequest;
        private RequestParser _parser;
        private HttpRequest _request;
        private RequestStateStore _requestStateStore;

        public ProgressWorkerRequest(HttpWorkerRequest wr, HttpRequest request)
        {
            this._originalWorkerRequest = wr;
            this._request = request;
            this._boundary = this.GetBoundary(this._request);
            this._requestStateStore = new RequestStateStore(this._request.ContentEncoding);
        }

        public override void CloseConnection()
        {
            this._originalWorkerRequest.CloseConnection();
        }

        public override void EndOfRequest()
        {
            this._originalWorkerRequest.EndOfRequest();
        }

        public override void FlushResponse(bool finalFlush)
        {
            this._originalWorkerRequest.FlushResponse(finalFlush);
        }

        public override string GetAppPath()
        {
            return this._originalWorkerRequest.GetAppPath();
        }

        public override string GetAppPathTranslated()
        {
            return this._originalWorkerRequest.GetAppPathTranslated();
        }

        public override string GetAppPoolID()
        {
            return this._originalWorkerRequest.GetAppPoolID();
        }

        private byte[] GetBoundary(HttpRequest request)
        {
            string contentType = request.ContentType;
            int index = contentType.IndexOf("boundary=");
            if (index <= 0)
            {
                return null;
            }
            return request.ContentEncoding.GetBytes("--" + contentType.Substring(index + "boundary=".Length));
        }

        public override long GetBytesRead()
        {
            return this._originalWorkerRequest.GetBytesRead();
        }

        public override byte[] GetClientCertificate()
        {
            return this._originalWorkerRequest.GetClientCertificate();
        }

        public override byte[] GetClientCertificateBinaryIssuer()
        {
            return this._originalWorkerRequest.GetClientCertificateBinaryIssuer();
        }

        public override int GetClientCertificateEncoding()
        {
            return this._originalWorkerRequest.GetClientCertificateEncoding();
        }

        public override byte[] GetClientCertificatePublicKey()
        {
            return this._originalWorkerRequest.GetClientCertificatePublicKey();
        }

        public override DateTime GetClientCertificateValidFrom()
        {
            return this._originalWorkerRequest.GetClientCertificateValidFrom();
        }

        public override DateTime GetClientCertificateValidUntil()
        {
            return this._originalWorkerRequest.GetClientCertificateValidUntil();
        }

        public override long GetConnectionID()
        {
            return this._originalWorkerRequest.GetConnectionID();
        }

        public override string GetFilePath()
        {
            return this._originalWorkerRequest.GetFilePath();
        }

        public override string GetFilePathTranslated()
        {
            return this._originalWorkerRequest.GetFilePathTranslated();
        }

        public override int GetHashCode()
        {
            return this._originalWorkerRequest.GetHashCode();
        }

        public override string GetHttpVerbName()
        {
            return this._originalWorkerRequest.GetHttpVerbName();
        }

        public override string GetHttpVersion()
        {
            return this._originalWorkerRequest.GetHttpVersion();
        }

        public override string GetKnownRequestHeader(int index)
        {
            return this._originalWorkerRequest.GetKnownRequestHeader(index);
        }

        public override string GetLocalAddress()
        {
            return this._originalWorkerRequest.GetLocalAddress();
        }

        public override int GetLocalPort()
        {
            return this._originalWorkerRequest.GetLocalPort();
        }

        public override string GetPathInfo()
        {
            return this._originalWorkerRequest.GetPathInfo();
        }

        public override byte[] GetPreloadedEntityBody()
        {
            byte[] preloadedEntityBody = this._originalWorkerRequest.GetPreloadedEntityBody();
            if (preloadedEntityBody != null)
            {
                this.UpdateProgress(preloadedEntityBody, preloadedEntityBody.Length);
            }
            return preloadedEntityBody;
        }

        public override int GetPreloadedEntityBody(byte[] buffer, int offset)
        {
            int preloadedEntityBody = this._originalWorkerRequest.GetPreloadedEntityBody(buffer, offset);
            this.UpdateProgress(buffer, preloadedEntityBody);
            return preloadedEntityBody;
        }

        public override int GetPreloadedEntityBodyLength()
        {
            return this._originalWorkerRequest.GetPreloadedEntityBodyLength();
        }

        public override string GetProtocol()
        {
            return this._originalWorkerRequest.GetProtocol();
        }

        public override string GetQueryString()
        {
            return this._originalWorkerRequest.GetQueryString();
        }

        public override byte[] GetQueryStringRawBytes()
        {
            return this._originalWorkerRequest.GetQueryStringRawBytes();
        }

        public override string GetRawUrl()
        {
            return this._originalWorkerRequest.GetRawUrl();
        }

        public override string GetRemoteAddress()
        {
            return this._originalWorkerRequest.GetRemoteAddress();
        }

        public override string GetRemoteName()
        {
            return this._originalWorkerRequest.GetRemoteName();
        }

        public override int GetRemotePort()
        {
            return this._originalWorkerRequest.GetRemotePort();
        }

        public override int GetRequestReason()
        {
            return this._originalWorkerRequest.GetRequestReason();
        }

        public override string GetServerName()
        {
            return this._originalWorkerRequest.GetServerName();
        }

        public override string GetServerVariable(string name)
        {
            return this._originalWorkerRequest.GetServerVariable(name);
        }

        public override int GetTotalEntityBodyLength()
        {
            return this._originalWorkerRequest.GetTotalEntityBodyLength();
        }

        public override string GetUnknownRequestHeader(string name)
        {
            return this._originalWorkerRequest.GetUnknownRequestHeader(name);
        }

        public override string[][] GetUnknownRequestHeaders()
        {
            return this._originalWorkerRequest.GetUnknownRequestHeaders();
        }

        public override string GetUriPath()
        {
            return this._originalWorkerRequest.GetUriPath();
        }

        public override long GetUrlContextID()
        {
            return this._originalWorkerRequest.GetUrlContextID();
        }

        public override IntPtr GetUserToken()
        {
            return this._originalWorkerRequest.GetUserToken();
        }

        public override IntPtr GetVirtualPathToken()
        {
            return this._originalWorkerRequest.GetVirtualPathToken();
        }

        public override bool HeadersSent()
        {
            return this._originalWorkerRequest.HeadersSent();
        }

        public override bool IsClientConnected()
        {
            return this._originalWorkerRequest.IsClientConnected();
        }

        public override bool IsEntireEntityBodyIsPreloaded()
        {
            return this._originalWorkerRequest.IsEntireEntityBodyIsPreloaded();
        }

        public override bool IsSecure()
        {
            return this._originalWorkerRequest.IsSecure();
        }

        public override string MapPath(string virtualPath)
        {
            return this._originalWorkerRequest.MapPath(virtualPath);
        }

        public override int ReadEntityBody(byte[] buffer, int size)
        {
            int validBytes = this._originalWorkerRequest.ReadEntityBody(buffer, size);
            this.UpdateProgress(buffer, validBytes);
            return validBytes;
        }

        public override int ReadEntityBody(byte[] buffer, int offset, int size)
        {
            int validBytes = this._originalWorkerRequest.ReadEntityBody(buffer, offset, size);
            this.UpdateProgress(buffer, validBytes);
            return validBytes;
        }

        public override void SendCalculatedContentLength(int contentLength)
        {
            this._originalWorkerRequest.SendCalculatedContentLength(contentLength);
        }

        public override void SendCalculatedContentLength(long contentLength)
        {
            this._originalWorkerRequest.SendCalculatedContentLength(contentLength);
        }

        public override void SendKnownResponseHeader(int index, string value)
        {
            this._originalWorkerRequest.SendKnownResponseHeader(index, value);
        }

        public override void SendResponseFromFile(IntPtr handle, long offset, long length)
        {
            this._originalWorkerRequest.SendResponseFromFile(handle, offset, length);
        }

        public override void SendResponseFromFile(string filename, long offset, long length)
        {
            this._originalWorkerRequest.SendResponseFromFile(filename, offset, length);
        }

        public override void SendResponseFromMemory(byte[] data, int length)
        {
            this._originalWorkerRequest.SendResponseFromMemory(data, length);
        }

        public override void SendResponseFromMemory(IntPtr data, int length)
        {
            this._originalWorkerRequest.SendResponseFromMemory(data, length);
        }

        public override void SendStatus(int statusCode, string statusDescription)
        {
            this._originalWorkerRequest.SendStatus(statusCode, statusDescription);
        }

        public override void SendUnknownResponseHeader(string name, string value)
        {
            this._originalWorkerRequest.SendUnknownResponseHeader(name, value);
        }

        public override void SetEndOfSendNotification(HttpWorkerRequest.EndOfSendNotification callback, object extraData)
        {
            this._originalWorkerRequest.SetEndOfSendNotification(callback, extraData);
        }

        protected virtual void UpdateProgress(byte[] buffer, int validBytes)
        {
            this.Parser.Parse(buffer, validBytes);
            if (this.RequestStateStore.CurrentRequestBytesCount >= this._request.ContentLength)
            {
                this.RequestStateStore.UploadComplete = true;
            }
        }

        private byte[] Boundary
        {
            get
            {
                return this._boundary;
            }
        }

        public override string MachineConfigPath
        {
            get
            {
                return this._originalWorkerRequest.MachineConfigPath;
            }
        }

        public override string MachineInstallDirectory
        {
            get
            {
                return this._originalWorkerRequest.MachineInstallDirectory;
            }
        }

        private RequestParser Parser
        {
            get
            {
                if (this._parser == null)
                {
                    this._parser = new RequestParser(this.Boundary, this._request.ContentEncoding, this.RequestStateStore);
                }
                return this._parser;
            }
        }

        internal RequestStateStore RequestStateStore
        {
            get
            {
                return this._requestStateStore;
            }
        }

        public override Guid RequestTraceIdentifier
        {
            get
            {
                return this._originalWorkerRequest.RequestTraceIdentifier;
            }
        }

        public override string RootWebConfigPath
        {
            get
            {
                return this._originalWorkerRequest.RootWebConfigPath;
            }
        }
    }
}