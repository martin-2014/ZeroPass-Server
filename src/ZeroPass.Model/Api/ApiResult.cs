using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ZeroPass.Model.Api
{
    public class ApiResult
    {
        public static ObjectResult Success<T>(T value) 
            => new OkObjectResult(new ApiResultSuccess<T> { Payload = value, Error = new EmptyObject { } });

        public static ObjectResult Success() 
            => new OkObjectResult(new ApiResultVoidSuccess 
            { 
                Payload = new EmptyObject { }, 
                Error = new EmptyObject { } 
            });

        public static ObjectResult Error(string errorId, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new ObjectResult(new ApiResultVoidError 
            { 
                Payload = new EmptyObject { }, 
                Error = new ErrorType { Id = errorId } 
            }) 
            { 
                StatusCode = (int)statusCode 
            };

        public static ObjectResult Error<T>(string errorId, T payload, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new ObjectResult(new ApiResultError<T> 
            { 
                Payload = payload, 
                Error = new ErrorType { Id = errorId } 
            }) 
            { 
                StatusCode = (int)statusCode 
            };
    }
}
