namespace ZeroPass.Model.Models
{
    public class DataKeyModel
    {
        public string PublicKey { get; set; }
        public string SelfPrivateKey { get; set; }
        public string AssignerPrivateKey { get; set; }
        public int AssignerId { get; set; }
    }
}