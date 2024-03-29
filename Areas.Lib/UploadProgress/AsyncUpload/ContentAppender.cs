// Generated by Reflector from D:\eConsular\src\Areas\Lib\RadWebUI\Areas.Lib.UploadProgress.dll

using System.IO;

namespace Areas.Lib.UploadProgress.AsyncUpload
{
    internal class ContentAppender : ITempFileAppender
    {
        private int appendedContentLength;
        private Stream content;

        public ContentAppender()
        {
        }

        public ContentAppender(Stream content)
        {
            this.content = content;
        }

        public void AppendTo(string fullPath)
        {
            using (FileStream stream = new FileStream(fullPath, FileMode.Append, FileAccess.Write))
            {
                using (this.content)
                {
                    this.appendedContentLength = (int)(stream.Length + this.content.Length);
                    StreamExtensions.CopyTo(this.content, stream);
                }
            }
        }

        public int AppendedContentLength
        {
            get
            {
                return this.appendedContentLength;
            }
        }
    }
}
