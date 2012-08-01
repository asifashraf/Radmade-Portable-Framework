using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Areas.Lib.HttpModules.FileUploadHelper
{
    public sealed class UploadedFileCollection : CollectionBase
    {
        internal UploadedFileCollection()
        {
        }

        internal UploadedFile Add(UploadedFile obj)
        {
            base.InnerList.Add(obj);
            return obj;
        }

        internal UploadedFile Remove(UploadedFile obj)
        {
            foreach (UploadedFile file in base.InnerList)
            {
                if (file == obj)
                {
                    base.InnerList.Remove(obj);
                    return obj;
                }
            }
            return obj;
        }

        public UploadedFile this[string id]
        {
            get
            {
                foreach (UploadedFile file in base.InnerList)
                {
                    if (file.InputFieldName == id)
                    {
                        return file;
                    }
                }
                return null;
            }
        }

        public UploadedFile this[int index]
        {
            get
            {
                return (UploadedFile)base.InnerList[index];
            }
        }
    }
}