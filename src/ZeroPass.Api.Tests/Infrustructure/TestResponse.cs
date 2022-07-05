using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System.Net;

namespace ZeroPass.Api.Tests
{
    public partial class TestResponse
    {
        public APIGatewayProxyResponse Origin { get; }
        public int StatusCode { get; }
        public TestResponseBody Body { get; }

        public TestResponse(APIGatewayProxyResponse response)
        {
            Origin = response;
            StatusCode = response.StatusCode;
            Body = string.IsNullOrEmpty(response.Body) ?
                new TestResponseBody() :
                JsonConvert.DeserializeObject<TestResponseBody>(response.Body);
        }

        public bool IsSuccess => StatusCode == (int)HttpStatusCode.OK && string.IsNullOrEmpty(Body.Error?.Id);
    }
}
