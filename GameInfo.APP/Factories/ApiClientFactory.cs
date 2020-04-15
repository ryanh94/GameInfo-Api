using GameInfo.APP.Interfaces;
using RestSharp;

namespace GameInfo.APP.Factories
{
    public class APIClientFactory : IAPIClientFactory
    {
        public IRestClient Create(string baseUrl)
        {
            return new RestClient(baseUrl);
        }
    }
}
