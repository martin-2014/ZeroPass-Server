namespace ZeroPass.Model.Models
{
    public class ClientVersionModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Edition { get; set; }
        public string Version { get; set; }
        public string DeviceId { get; set; }
    }
}