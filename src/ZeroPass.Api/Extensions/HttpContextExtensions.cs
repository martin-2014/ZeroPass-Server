using Microsoft.AspNetCore.Http;

namespace ZeroPass.Api
{
    public static class HttpContextExtensions
    {
        const string KeyName = "X-Request-ID";

        public static string GetRequestId(this HttpContext context)
        {
            return context.Items[KeyName] as string;
        }
    }
}
