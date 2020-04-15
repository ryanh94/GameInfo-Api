using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Core.Model
{
    public class AppSettings
    {
        public string Connection { get; set; }
        public TokenSettings TokenSettings { get; set; }
        public List<IgnoreLoggingUrls> IgnoreLoggingUrls { get; set; }
    }
    public class TokenSettings
    {
        public string Key { get; set; }
        public int ExpiryTime { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
    public class IgnoreLoggingUrls
    {
        public string Url { get; set; }
    }
}
