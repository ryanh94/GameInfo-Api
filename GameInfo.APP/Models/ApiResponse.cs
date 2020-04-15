using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GameInfo.APP.Models
{
    public class ApiResponse
    {
        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public IList<RestResponseCookie> Cookies { get; set; }

    }
}
