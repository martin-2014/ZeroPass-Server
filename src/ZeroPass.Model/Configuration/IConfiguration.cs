namespace ZeroPass.Model.Configuration
{
    public interface IConfiguration
    {
        string GetValue(string key);

        int GetIntegerValue(string key, int defaultValue);
    }
}
