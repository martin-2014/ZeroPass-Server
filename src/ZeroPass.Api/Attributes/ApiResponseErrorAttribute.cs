using Swashbuckle.AspNetCore.Annotations;
using System;
using ZeroPass.Model.Api;

namespace ZeroPass.Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiResponseErrorAttribute : SwaggerResponseAttribute
    {
        const string newline = "<br/>";

        string errorList;

        public ApiResponseErrorAttribute(params string[] errorIds) :
            base(ApiErrorCode.DefaultStatusCode, null, typeof(ApiResultVoidError))
            => InitErrorInfo(errorIds);

        public ApiResponseErrorAttribute(Type type, params string[] errorIds) :
            base(ApiErrorCode.DefaultStatusCode, null, type)
        {
            if (type != null)
            {
                var typeInfo = typeof(ApiResultError<>);
                Type = typeInfo.MakeGenericType(new Type[] { type });
            }
            else
            {
                Type = typeof(ApiResultVoidError);
            }

            InitErrorInfo(errorIds);
        }

        public ApiResponseErrorAttribute(int statusCode) :
            base(statusCode)
        {

        }

        string description;
        public new string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                base.Description = $"{description}{newline}{errorList}";
            }
        }

        void InitErrorInfo(string[] errorIds)
        {
            ContentTypes = new string[] { "application/json" };
            errorList += string.IsNullOrEmpty(errorList) ? "" : newline;
            errorList += $"Error List:{newline}";
            foreach (var errorId in errorIds)
            {
                errorList += $"{Resources.ResourceManager.GetString(errorId, Resources.Culture)}{newline}";
            }
            base.Description = errorList;
        }
    }

    public class ApiErrorCode
    {
        public const int DefaultStatusCode = 2000;
    }
}
