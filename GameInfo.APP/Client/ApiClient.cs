using GameInfo.APP.Interfaces;
using GameInfo.APP.Models;
using RestSharp;
using System.Threading.Tasks;

namespace GameInfo.APP.Client
{
    public class ApiClient : IApiClient
    {
        private readonly IAPIClientFactory _clientFactory;
        private readonly IAPIRequestFactory _requestFactory;
        public ApiClient(IAPIClientFactory clientFactory, IAPIRequestFactory requestFactory)
        {
            _clientFactory = clientFactory;
            _requestFactory = requestFactory;
        }

        public async Task<ApiResponse> ExecuteAsync(ApiRequest request)
        {
            var client = _clientFactory.Create(request.BaseUri);
            var restRequest = _requestFactory.Create(request.Resource, request.Method);
            restRequest.RequestFormat = DataFormat.Json;

            foreach (var item in request.Headers)
            {
                restRequest.AddHeader(item.Key, item.Value);
            }

            foreach (var item in request.BodyParameters)
            {
                restRequest.AddParameter(item.Key, item.Value);
            }

            foreach (var item in request.UriParameters)
            {
                restRequest.AddParameter(item.Key, item.Value, ParameterType.UrlSegment);
            }

            foreach (var item in request.QueryStringParameters)
            {
                restRequest.AddParameter(item.Key, item.Value, ParameterType.QueryStringWithoutEncode);
            }

            foreach (var item in request.Cookies)
            {
                restRequest.AddCookie(item.Key, item.Value);
            }

            if (request.Body != null)
            {
                restRequest.AddBody(request.Body);
            }

            var response = await client.ExecuteTaskAsync(restRequest);

            return new ApiResponse { Cookies = response.Cookies, Content = response.Content, StatusCode = response.StatusCode };
        }

        public async Task<T> ExecuteAsync<T>(ApiRequest request)
        {
            var client = _clientFactory.Create(request.BaseUri);
            var restRequest = _requestFactory.Create(request.Resource, request.Method);
            foreach (var item in request.Headers)
            {
                restRequest.AddHeader(item.Key, item.Value);
            }

            foreach (var item in request.BodyParameters)
            {
                restRequest.AddParameter(item.Key, item.Value);
            }

            foreach (var item in request.UriParameters)
            {
                restRequest.AddParameter(item.Key, item.Value, ParameterType.UrlSegment);
            }

            foreach (var item in request.QueryStringParameters)
            {
                restRequest.AddParameter(item.Key, item.Value, ParameterType.QueryStringWithoutEncode);
            }

            if (request.Body != null)
            {
                restRequest.AddJsonBody(request.Body);
                //restRequest.RequestFormat = DataFormat.Json;
            }

            var response = await client.ExecuteTaskAsync(restRequest);
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}

