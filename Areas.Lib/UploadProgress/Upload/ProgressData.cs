// Generated by Reflector from D:\eConsular\src\Areas\Lib\RadWebUI\Areas.Lib.UploadProgress.dll
namespace Areas.Lib.UploadProgress.Upload
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using Areas.Lib.UploadProgress.Upload.AsyncUploadModels;

    public class ProgressData
    {
        private Dictionary<string, object> _items = new Dictionary<string, object>();
        private const string CurrentOperationTextKey = "CurrentOperationText";
        private const string OperationCompleteKey = "OperationComplete";
        private const string PrimaryPercentKey = "PrimaryPercent";
        private const string PrimaryTotalKey = "PrimaryTotal";
        private const string PrimaryValueKey = "PrimaryValue";
        private object progressLock = new object();
        private const string SecondaryPercentKey = "SecondaryPercent";
        private const string SecondaryTotalKey = "SecondaryTotal";
        private const string SecondaryValueKey = "SecondaryValue";
        private const string SpeedKey = "Speed";
        private const string TimeElapsedKey = "TimeElapsed";
        private const string TimeEstimatedKey = "TimeEstimated";

        protected string FormatString(string formatee)
        {
            return formatee.Replace(@"\", @"\\").Replace("'", @"\'");
        }

        public virtual void Serialize(TextWriter writer)
        {
            writer.Write("var rawProgressData = {");
            if (this._items.Keys.Count > 0)
            {
                writer.Write("InProgress:true");
            }
            else
            {
                writer.Write("InProgress:false");
            }
            bool flag2 = this._items.Keys.Count > 0;
            writer.Write(",ProgressCounters:{0}", flag2.ToString().ToLower());
            lock (this.progressLock)
            {
                foreach (string str in this._items.Keys)
                {
                    writer.Write(",");
                    writer.Write(str);
                    writer.Write(":'");
                    this.WriteValue(writer, str);
                    writer.Write("'");
                }
                this.SerializeCustomData(writer);
            }
            writer.Write("};");
        }

        public string SerializeToString(UploadTimer timer)
        {
            var writer = new StringBuilder();
            writer.Append("{");
            if (this._items.Keys.Count > 0)
            {
                writer.Append("InProgress:true");
            }
            else
            {
                writer.Append("InProgress:false");
            }
            bool flag2 = this._items.Keys.Count > 0;
            writer.Append( String.Format(",ProgressCounters:{0}", flag2.ToString().ToLower()) );
            lock (this.progressLock)
            {
                foreach (string str in this._items.Keys)
                {
                    writer.Append(",");
                    writer.Append(str);
                    writer.Append(":'");
                    this.WriteValue(writer, str);
                    writer.Append("'");
                }
                this.SerializeCustomData(writer);
            }
            writer.Append("}");
            return writer.ToString();
        }

        protected virtual void SerializeCustomData(TextWriter writer)
        {
        }

        protected virtual void SerializeCustomData(StringBuilder writer)
        {
        }

        private void WriteValue(TextWriter writer, string key)
        {
            object obj2 = this._items[key];
            if (obj2 is bool)
            {
                writer.Write(obj2.ToString().ToLower());
            }
            else if (obj2 is int)
            {
                writer.Write(obj2);
            }
            else
            {
                writer.Write(this.FormatString(obj2.ToString()));
            }
        }

        private void WriteValue(StringBuilder writer, string key)
        {
            object obj2 = this._items[key];
            if (obj2 is bool)
            {
                writer.Append(obj2.ToString().ToLower());
            }
            else if (obj2 is int)
            {
                writer.Append(obj2);
            }
            else
            {
                writer.Append(this.FormatString(obj2.ToString()));
            }
        }

        public virtual object CurrentOperationText
        {
            get
            {
                return this["CurrentOperationText"];
            }
            set
            {
                this["CurrentOperationText"] = value;
            }
        }

        public object this[string key]
        {
            get
            {
                lock (this.progressLock)
                {
                    if (this._items.ContainsKey(key))
                    {
                        return this._items[key];
                    }
                }
                return null;
            }
            set
            {
                lock (this.progressLock)
                {
                    this._items[key] = value;
                }
            }
        }

        public virtual bool OperationComplete
        {
            get
            {
                if (this["OperationComplete"]== null)
                {
                    return false;
                }
                var res = Convert.ToBoolean(this["OperationComplete"]);
                return res;
            }
            set
            {
                this["OperationComplete"] = value;
            }
        }

        public virtual object PrimaryPercent
        {
            get
            {
                return this["PrimaryPercent"];
            }
            set
            {
                this["PrimaryPercent"] = value;
            }
        }

        public virtual object PrimaryTotal
        {
            get
            {
                return this["PrimaryTotal"];
            }
            set
            {
                this["PrimaryTotal"] = value;
            }
        }

        public virtual object PrimaryValue
        {
            get
            {
                return this["PrimaryValue"];
            }
            set
            {
                this["PrimaryValue"] = value;
            }
        }

        public virtual object SecondaryPercent
        {
            get
            {
                return this["SecondaryPercent"];
            }
            set
            {
                this["SecondaryPercent"] = value;
            }
        }

        public virtual object SecondaryTotal
        {
            get
            {
                return this["SecondaryTotal"];
            }
            set
            {
                this["SecondaryTotal"] = value;
            }
        }

        public virtual object SecondaryValue
        {
            get
            {
                return this["SecondaryValue"];
            }
            set
            {
                this["SecondaryValue"] = value;
            }
        }

        public virtual object Speed
        {
            get
            {
                return this["Speed"];
            }
            set
            {
                this["Speed"] = value;
            }
        }

        public virtual object TimeElapsed
        {
            get
            {
                return this["TimeElapsed"];
            }
            set
            {
                this["TimeElapsed"] = value;
            }
        }

        public virtual object TimeEstimated
        {
            get
            {
                return this["TimeEstimated"];
            }
            set
            {
                this["TimeEstimated"] = value;
            }
        }
    }
}
