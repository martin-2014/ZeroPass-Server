using System;
using ZeroPass.Model.Configuration;

namespace ZeroPass.Service.Configuration
{
    public class EnvironmentConfig : IConfiguration
    {
        public int GetIntegerValue(string key, int defaultValue)
        {
            var stringValue = GetValue(key);
            return int.TryParse(stringValue, out var value) ? value : defaultValue;
        }

        public string GetValue(string key)
            => Environment.GetEnvironmentVariable(key);
    }
}
