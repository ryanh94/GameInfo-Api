using GameInfo.APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInfo.APP.Interfaces
{
    public interface IApiClient
    {
        Task<ApiResponse> ExecuteAsync(ApiRequest request);
        Task<T> ExecuteAsync<T>(ApiRequest request);
    }
}
