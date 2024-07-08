using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Shared
{
    public  class OpenTelemetryTraceIdMiddleware
    {
        private readonly RequestDelegate _next;

        public OpenTelemetryTraceIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var logger = context.RequestServices.GetService<ILogger<OpenTelemetryTraceIdMiddleware>>();
            using (logger.BeginScope("{$trceId}", Activity.Current?.TraceId.ToString()))
            {
                await _next(context);
            }
         
      
        }
    }
}
