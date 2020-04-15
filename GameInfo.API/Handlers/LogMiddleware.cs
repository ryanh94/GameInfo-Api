using GameInfo.Core.Interfaces;
using GameInfo.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInfo.API.Handlers
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;
        private readonly IOptions<AppSettings> _settings;
        public LogMiddleware(RequestDelegate next, ILoggerService loggerFactory, IOptions<AppSettings> settings)
        {
            _next = next;
            _logger = loggerFactory;
            _settings = settings;
        }


        public async Task Invoke(HttpContext context)
        {
          
            if (context.Request.Path.HasValue && !_settings.Value.IgnoreLoggingUrls.Any(x => context.Request.Path.Value.ToLower().Contains(x.Url.ToLower())))
            {
                var request = await FormatRequest(context.Request);


                var originalBodyStream = context.Response.Body;


                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;


                    await _next(context);


                    var response = await FormatResponse(context.Response);


                    await responseBody.CopyToAsync(originalBodyStream);


                    var claims = context.Response.HttpContext.User.Claims;
                    var userid = claims.Where(c => c.Type.ToLower() == "userid").FirstOrDefault()?.Value;
                    await _logger.LogRequest(Convert.ToInt32(userid), request, $"{context.Response.StatusCode}//" + $"Response Body: {response}");
                }
            }

            await _next(context);
        }
        private async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;


            request.EnableBuffering();
            body.Position = 0;


            var buffer = new byte[Convert.ToInt32(request.ContentLength)];


            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            body.Position = 0;


            var bodyAsText = Encoding.UTF8.GetString(buffer);


            request.Body = body;


            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }
        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);


            string text = await new StreamReader(response.Body).ReadToEndAsync();


            response.Body.Seek(0, SeekOrigin.Begin);


            return $"{response.StatusCode}: {text}";
        }
    }
}

