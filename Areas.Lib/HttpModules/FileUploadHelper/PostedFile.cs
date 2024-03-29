﻿namespace Areas.Lib.HttpModules.FileUploadHelper
{
    using System;
    using System.IO;
    using System.Web;

    internal sealed class PostedFile : UploadedFile
    {
        private HttpPostedFile _file;
        private string _inputFieldName;

        internal PostedFile(string inputFieldName, HttpPostedFile file)
        {
            this._inputFieldName = inputFieldName;
            this._file = file;
        }

        public override void SaveAs(string fileName, bool overwrite)
        {
            if (overwrite || (!overwrite && !File.Exists(fileName)))
            {
                this._file.SaveAs(fileName);
            }
        }

        public override int ContentLength
        {
            get
            {
                return this._file.ContentLength;
            }
        }

        public override string ContentType
        {
            get
            {
                return this._file.ContentType;
            }
        }

        public override string FileName
        {
            get
            {
                return this._file.FileName;
            }
        }

        protected internal override string InputFieldName
        {
            get
            {
                return this._inputFieldName;
            }
        }

        public override Stream InputStream
        {
            get
            {
                return this._file.InputStream;
            }
        }
    }

}