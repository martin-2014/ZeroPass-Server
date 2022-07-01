namespace ZeroPass.Model.Models
{
    public partial class DataKeyModel
    {
        public string PublicKey { get; set; }
        public string SelfPrivateKey { get; set; }
        public int AssignerId { get; set; }
    }
}