using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ZeroPass.Api.Properties;
using ZeroPass.Model.Api;
using ZeroPass.Model.Logging;

namespace ZeroPass.Api
{
    public class ErrorHandlerMiddleware
    {
        readonly RequestDelegate Next;

        public ErrorHandlerMiddleware(RequestDelegate next)
            => Next = next;

        static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task InvokeAsync(HttpContext context, ISharedLogger logger)
        {
            try
            {
                await Next(context);
            }
            catch (Exception error)
            {
                var requestId = context.GetRequestId();
                logger.LogError($"Error in request {requestId}: {error}");
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = ApiResult.Error(Resources.ErrorServerError);
                var text = JsonSerializer.Serialize(result.Value, JsonSerializerOptions);
                await response.WriteAsync(text);
            }
        }
    }
}
