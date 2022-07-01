using System;

namespace ZeroPass.Model.Models
{
    [Serializable]
    public class UserKeySessionModel
    {
        public int Id;

        public string Email;

        public string Salt;

        public string Verifier;

        public string ServerPrivateKey;

        public string ClientPublicKey;

        public string CommunicateKey { get; set; }

        public string MasterKey { get; set; }
    }
}