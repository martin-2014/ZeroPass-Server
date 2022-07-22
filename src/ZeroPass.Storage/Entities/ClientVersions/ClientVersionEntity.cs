namespace ZeroPass.Storage.Entities.ClientVersions
{
    public class ClientVersionEntity
    {
        public int UserId { get; set; }
        public string Edition { get; set; }
        public string Version { get; set; }
        public string DeviceId { get; set; }
    }
}