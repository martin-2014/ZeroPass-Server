using System;

namespace ZeroPass.Storage.Entities
{
    [Serializable]
    public class UserKeyEntity
    {
        public int UserId { get; set; }

        public string Salt { get; set; }

        public string Verifier { get; set; }

        public string PublicDataKey { get; set; }

        public string PrivateDataKey { get; set; }
    }
}
