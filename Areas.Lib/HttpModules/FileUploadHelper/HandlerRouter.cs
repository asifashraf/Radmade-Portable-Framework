namespace Areas.Lib.HttpModules.FileUploadHelper
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    internal class HandlerRouter
    {
        private Dictionary<string, TFunc<IHttpHandler>> _handlers = new Dictionary<string, TFunc<IHttpHandler>>(StringComparer.InvariantCultureIgnoreCase);

        private string ExtractKey(HttpContext context)
        {
            return context.Request[HandlerUrlKey];
        }

        protected virtual void PopulateHandlers()
        {
            //this.Handlers.Add(RadBinaryImage.HandlerRouterKey, () => new RadBinaryImageHandler());
            //this.Handlers.Add(RadCaptcha.HandlerRouterKey, () => new CaptchaImageHandler());
            //this.Handlers.Add(RadAsyncUpload.HandlerRouterKey, () => new AsyncUploadHandler());
            //this.Handlers.Add(RadCaptcha.HandlerRouterKeyCaptchaAudio, () => new CaptchaAudioHandler());
            //this.Handlers.Add(RadImageEditor.HandlerRouterKey, () => new ImageEditorCacheHandler());
        }

        public bool ProcessHandler(HttpContext context)
        {
            string str = this.ExtractKey(context);
            return (!string.IsNullOrEmpty(str) && this.ProcessHandler(str, context));
        }

        public bool ProcessHandler(string handlerKey, HttpContext context)
        {
            if (string.IsNullOrEmpty(handlerKey))
            {
                throw new ArgumentNullException("handlerKey");
            }
            this.PopulateHandlers();
            if (this._handlers.ContainsKey(handlerKey))
            {
                this._handlers[handlerKey]().ProcessRequest(context);
                return true;
            }
            return false;
        }

        protected Dictionary<string, TFunc<IHttpHandler>> Handlers
        {
            get
            {
                return this._handlers;
            }
        }

        internal static string HandlerUrlKey
        {
            get
            {
                return "type";
            }
        }
    }
}