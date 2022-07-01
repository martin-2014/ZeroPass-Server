using System.Text.Json;

namespace ZeroPass.Storage.Fakes
{
    public static class JsonElementExtensions
    {
        public static T To<T>(this JsonElement json)
        {
            return JsonSerializer.Deserialize<T>(json.GetRawText());
        }
    }
}