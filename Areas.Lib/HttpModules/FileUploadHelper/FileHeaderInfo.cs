namespace Areas.Lib.HttpModules.FileUploadHelper
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class FileHeaderInfo : FieldHeaderInfo
    {
        private string _contentType;
        private string _fileName;
        private static Regex _fileNameExtractor = new Regex("\\bfilename=(\"?)([^;\\r\\n]*)\\1", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public FileHeaderInfo(byte[] headerContent, Encoding encoding)
            : base(headerContent, encoding)
        {
        }

        public static bool IsFileHeaderInfo(byte[] headerContent, Encoding encoding)
        {
            string input = encoding.GetString(headerContent).ToLower();
            return _fileNameExtractor.IsMatch(input);
        }

        public virtual string ContentType
        {
            get
            {
                if (this._contentType == null)
                {
                    Regex regex = new Regex("\\bContent-Type: ?(\"?)([^;\\r\\n]*)\\1", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    this._contentType = regex.Match(base.ContentAsString).Groups[2].Value;
                }
                return this._contentType;
            }
        }

        public virtual string FileName
        {
            get
            {
                if (this._fileName == null)
                {
                    this._fileName = _fileNameExtractor.Match(base.ContentAsString).Groups[2].Value;
                }
                return this._fileName;
            }
        }
    }
}