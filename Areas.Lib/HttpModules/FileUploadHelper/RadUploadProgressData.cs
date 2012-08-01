namespace Areas.Lib.HttpModules.FileUploadHelper
{
    using System.IO;

    internal class RadUploadProgressData : ProgressData
    {
        private bool _checkHandlerRegistration = true;
        private int _completeBytes;
        private int _requestLength;

        protected override void SerializeCustomData(TextWriter writer)
        {
            base.SerializeCustomData(writer);
            if (this.CheckHandlerRegistration && !RadUploadHttpModule.IsRegistered)
            {
                writer.Write(@",ProgressError:'RadUpload Ajax callback error: Cannot find RadUploadHttpModule.\r\n\r\nDid you register the RadUploadHttpModule?\r\n\r\nIf you do not register the HttpModule you cannot benefit from RadMemoryOptimization and RadProgressArea.\r\n\r\nPlease, see the help for more details: RadUpload - Using RadUpload - Configuration - RadUploadHttpModule.'");
            }
            writer.Write(",RadUpload:{");
            writer.Write("RequestSize:");
            writer.Write(this.RequestLength);
            writer.Write(",");
            writer.Write("Bytes:");
            writer.Write(this.CompleteBytes);
            writer.Write(",");
            writer.Write("FilesCount:");
            writer.Write(this.SecondaryValue);
            writer.Write(",");
            writer.Write("CurrentFileName:'");
            writer.Write(base.FormatString(this.CurrentFileName));
            writer.Write("',");
            writer.Write("RequestLength:");
            writer.Write(this.RequestLength);
            writer.Write("}");
        }

        internal bool CheckHandlerRegistration
        {
            get
            {
                return this._checkHandlerRegistration;
            }
            set
            {
                this._checkHandlerRegistration = value;
            }
        }

        public int CompleteBytes
        {
            get
            {
                return this._completeBytes;
            }
            set
            {
                this._completeBytes = value;
            }
        }

        private string CurrentFileName
        {
            get
            {
                if (this.CurrentOperationText == null)
                {
                    return string.Empty;
                }
                return this.CurrentOperationText.ToString();
            }
        }

        public int RequestLength
        {
            get
            {
                return this._requestLength;
            }
            set
            {
                this._requestLength = value;
            }
        }
    }
}