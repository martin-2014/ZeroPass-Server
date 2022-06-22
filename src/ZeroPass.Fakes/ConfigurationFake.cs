using System.Collections.Generic;
using ZeroPass.Model.Configuration;

namespace ZeroPass.Fakes
{
    public class ConfigurationFake : IConfiguration
    {
        readonly Dictionary<string, string> Configurations = new Dictionary<string, string>();

        public int GetIntegerValue(string key, int defaultValue)
            => defaultValue;

        public string GetValue(string key)
            => Configurations.TryGetValue(key, out var value) ? value : "";

        public void SetValue(string key, string value)
            => Configurations[key] = value;
    }
}
