using System;

namespace ZeroPass.Model.Models
{
    [Serializable]
    public class UserKeyExchangeModel
    {
        public string KeyId;

        public string ServerPrivateKey;

        public string ServerPublicKey;

        public string ClientPublicKey;
    }
}