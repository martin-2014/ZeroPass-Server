using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using ZeroPass.Model.Api;

namespace ZeroPass.Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiResponseSuccessAttribute : SwaggerResponseAttribute
    {
        public ApiResponseSuccessAttribute(Type type = null, string description = null)
            : base((int)HttpStatusCode.OK, description, type)
        {
            if (type != null)
            {
                var typeInfo = typeof(ApiResultSuccess<>);
                Type = typeInfo.MakeGenericType(new Type[] { type });
            }
            else
            {
                Type = typeof(ApiResultVoidSuccess);
            }

            ContentTypes = new string[] { "application/json" };
        }
    }
}
