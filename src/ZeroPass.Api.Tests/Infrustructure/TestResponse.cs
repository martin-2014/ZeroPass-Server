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

        public TestResponse(APIGatewayProxyResponse reponse)
        {
            Origin = reponse;
            StatusCode = reponse.StatusCode;
            Body = string.IsNullOrEmpty(reponse.Body) ?
                new TestResponseBody() :
                JsonConvert.DeserializeObject<TestResponseBody>(reponse.Body);
        }

        public bool IsSuccess => StatusCode == (int)HttpStatusCode.OK && string.IsNullOrEmpty(Body.Error?.Id);
    }
}
