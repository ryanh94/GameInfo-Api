using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInfo.APP.Interfaces
{
    public interface IAPIRequestFactory
    {
        IRestRequest Create(string url, Method method);
    }
}
