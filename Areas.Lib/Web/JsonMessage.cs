﻿namespace WebAreas.Lib.Web
{
    public class JsonMessage
    {
        public object message { get; set; }

        internal ResultType statusEnum { get; set; }

        public string status
        {
            get { return statusEnum.ToString(); }
        }

        public object data { get; set; }

        public string code { get; set; }

        public JsonMessage(
            ResultType messageStatus, string msg, object dataObject, string specialCode)
        {
            statusEnum = messageStatus;

            message = msg;

            data = dataObject;

            code = specialCode;
        }
    }
}
