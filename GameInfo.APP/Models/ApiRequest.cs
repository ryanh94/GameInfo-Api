using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInfo.APP.Models
{
    public class ApiRequest
    {
        public ApiRequest()
        {
            Headers = new Dictionary<string, string>();
            UriParameters = new Dictionary<string, object>();
            BodyParameters = new Dictionary<string, object>();
            QueryStringParameters = new Dictionary<string, object>();
            Cookies = new Dictionary<string, string>();
        }
        public string BaseUri { get; set; }
        public string Resource { get; set; }
        public Method Method { get; set; }

        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, object> UriParameters { get; set; }
        public Dictionary<string, object> BodyParameters { get; set; }
        public Dictionary<string, object> QueryStringParameters { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public object Body { get; set; }
    }
}
