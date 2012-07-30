using System;

namespace Areas.Lib.WebAuth
{
    public class Token
    {
        public string UserPrimaryKey { get; set; }

        public string UserNameOrEmail { get; set; }

        public string UserIPAddress { get; set; }

        public DateTime Time { get; set; }

        public DateTime Expiry { get; set; }

    }
}
