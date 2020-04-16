using GameInfo.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace GameInfo.Infrastructure.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }
        public void LogRequest(int userId, string requestBody, string responseBody)
        {
            _logger.LogTrace("HTTP Request, Created: {0}", JsonConvert.SerializeObject(new
            {
                Created = DateTimeOffset.UtcNow,
                Request = requestBody,
                Response = responseBody,
                UserId = userId
            }));
        }
    }
}
