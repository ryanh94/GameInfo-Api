using GameInfo.APP.Interfaces;
using RestSharp;

namespace GameInfo.APP.Factories
{
    public class APIRequestFactory : IAPIRequestFactory
    {
        public IRestRequest Create(string url, Method method)
        {
            return new RestRequest(url, method);
        }
    }
}
