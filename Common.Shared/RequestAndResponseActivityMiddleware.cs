using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Common.Shared
{
    public class RequestAndResponseActivityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestAndResponseActivityMiddleware(RequestDelegate next)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            await AddRequestBodyContentToActivityTags(httpContext);
            var originalResponseBodyStream = httpContext.Response.Body;

            using (var responseBodyMemoryStream = _recyclableMemoryStreamManager.GetStream())
            {
                httpContext.Response.Body = responseBodyMemoryStream;

                try
                {
                    await _next(httpContext);
                    await AddResponseBodyContentToActivityTags(httpContext, responseBodyMemoryStream);
                }
                finally
                {
                    httpContext.Response.Body = originalResponseBodyStream;
                }
            }
        }

        private async Task AddRequestBodyContentToActivityTags(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            var requestBodyStreamReader = new StreamReader(httpContext.Request.Body);
            var requestBodyContent = await requestBodyStreamReader.ReadToEndAsync();
            Activity.Current?.SetTag("http.request.body", requestBodyContent);
            httpContext.Request.Body.Position = 0;
        }

        private async Task AddResponseBodyContentToActivityTags(HttpContext httpContext, MemoryStream responseBodyMemoryStream)
        {
            responseBodyMemoryStream.Position = 0;
            var responseBodyContent = await new StreamReader(responseBodyMemoryStream).ReadToEndAsync();
            Activity.Current?.SetTag("http.response.body", responseBodyContent);
            responseBodyMemoryStream.Position = 0;

            await responseBodyMemoryStream.CopyToAsync(httpContext.Response.Body);
        }
    }
}
