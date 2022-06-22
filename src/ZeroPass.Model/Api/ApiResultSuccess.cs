namespace ZeroPass.Model.Api
{
    public class ApiResultSuccess<T>
    {
        public T Payload { get; set; }
        public EmptyObject Error { get; set; }
    }
}
