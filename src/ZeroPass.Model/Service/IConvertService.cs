namespace ZeroPass.Model.Service
{
    public interface IConvertService
    {
        public byte[] FromHexString(string hexString);
        public string ToHexString(byte[] bytes);

    }
}
