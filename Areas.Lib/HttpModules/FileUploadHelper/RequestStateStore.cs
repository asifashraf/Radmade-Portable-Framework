﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Areas.Lib.HttpModules.FileUploadHelper
{
    internal class RequestStateStore
    {
        private RequestField _currentField;
        private int _currentRequestBytesCount;
        private Encoding _encoding;
        private List<RequestField> _fields;
        private bool _uploadComplete;

        public RequestStateStore(Encoding encoding)
        {
            this._encoding = encoding;
        }

        public void Record(byte[] fieldContent, bool isFinal)
        {
            if (this._currentField == null)
            {
                this._currentField = new RequestField(this._encoding);
            }
            bool flag = this._currentField.Header != null;
            this._currentField.AddData(fieldContent, isFinal);
            if (!flag && (this._currentField.Header != null))
            {
                this.Fields.Add(this._currentField);
            }
            if (isFinal)
            {
                this._currentField = null;
            }
        }

        public void UpdateCurrentRequestBytesCount(int parsedBytesCount)
        {
            this._currentRequestBytesCount += parsedBytesCount;
        }

        public int CurrentRequestBytesCount
        {
            get
            {
                return this._currentRequestBytesCount;
            }
        }

        public List<RequestField> Fields
        {
            get
            {
                if (this._fields == null)
                {
                    this._fields = new List<RequestField>();
                }
                return this._fields;
            }
        }

        public bool HasOpenField
        {
            get
            {
                return (this._currentField != null);
            }
        }

        public RequestField LastHeaderCompleteField
        {
            get
            {
                if (this.Fields.Count > 0)
                {
                    return this.Fields[this.Fields.Count - 1];
                }
                return null;
            }
        }

        public bool UploadComplete
        {
            get
            {
                return this._uploadComplete;
            }
            set
            {
                this._uploadComplete = value;
            }
        }
    }
}