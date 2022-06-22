using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ZeroPass.Api
{
    public class RequestIdHandlerMiddleware
    {
        readonly RequestDelegate Next;
        const string KeyName = "X-Request-ID";

        public RequestIdHandlerMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = GetOrCreateRequestId(context);
            context.Items[KeyName] = requestId;
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[KeyName] = requestId;
                return Task.CompletedTask;
            });

            await Next(context);
        }

        string GetOrCreateRequestId(HttpContext context)
        {
            foreach (var kv in context.Request.Headers)
            {
                if (kv.Key.Equals(KeyName, StringComparison.OrdinalIgnoreCase))
                {
                    return kv.Value;
                }
            }

            return GenerateId();
        }

        static string GenerateId() => Guid.NewGuid().ToString("D");
    }
}
