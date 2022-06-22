namespace ZeroPass.Model.Api
{
    public class ApiResultError<T>
    {
        public T Payload { get; set; }
        public ErrorType Error { get; set; }
    }
}
