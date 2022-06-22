using Newtonsoft.Json;

namespace ZeroPass.Api.Tests
{
    public class TestResponseBody
    {
        public class ErrorInfo
        {
            public string Id { get; set; }
        }

        public object Payload { get; set; }
        public ErrorInfo Error { get; set; }

        public T Value<T>() => Payload == null 
            ? default 
            : JsonConvert.DeserializeObject<T>(Payload.ToString());
    }
}
