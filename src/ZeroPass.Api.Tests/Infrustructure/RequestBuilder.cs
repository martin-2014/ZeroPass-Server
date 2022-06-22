using Amazon.Lambda.APIGatewayEvents;

namespace ZeroPass.Api.Tests
{
    public class RequestBuilder
    {
        const string GET = "GET";
        const string POST = "POST";
        const string PUT = "PUT";
        const string DELETE = "DELETE";

        string Path { get; }
        string Method { get; }
        string Resource { get; } = "/{proxy+}";

        public static RequestBuilder GetRequest(string path)
            => new RequestBuilder(path, GET);

        public RequestBuilder(string path, string method)
        {
            Path = path;
            Method = method;
        }

        public APIGatewayProxyRequest Build()
        => new APIGatewayProxyRequest
        {
            Path = Path,
            HttpMethod = Method,
            Resource = Resource,
        };
    }
}
