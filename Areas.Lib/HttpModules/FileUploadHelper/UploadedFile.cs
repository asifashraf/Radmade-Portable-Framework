using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Areas.Lib.HttpModules.FileUploadHelper
{
    public abstract class UploadedFile
    {
        protected UploadedFile()
        {
        }

        public static UploadedFile FromHttpPostedFile(HttpPostedFile file)
        {
            return new PostedFile(string.Empty, file);
        }

        public static UploadedFile FromHttpPostedFile(string inputFieldName, HttpPostedFile file)
        {
            return new PostedFile(inputFieldName, file);
        }

        public string GetExtension()
        {
            return Path.GetExtension(this.FileName);
        }

        public virtual string GetFieldValue(string fieldName)
        {
            string str = new Regex(@"^([\w\d]+)file(\d+)$").Replace(this.InputFieldName, string.Format("$1{0}$2", fieldName));
            return HttpContext.Current.Request.Form[str];
        }

        public bool GetIsFieldChecked(string fieldName)
        {
            string fieldValue = this.GetFieldValue(fieldName);
            return ((fieldValue != null) && (fieldValue.Length > 0));
        }

        public string GetName()
        {
            return Path.GetFileName(this.FileName);
        }

        public string GetNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(this.FileName);
        }

        public void SaveAs(string fileName)
        {
            this.SaveAs(fileName, true);
        }

        public abstract void SaveAs(string fileName, bool overwrite);

        public abstract int ContentLength { get; }

        public abstract string ContentType { get; }

        public abstract string FileName { get; }

        protected internal abstract string InputFieldName { get; }

        public abstract Stream InputStream { get; }
    }
}