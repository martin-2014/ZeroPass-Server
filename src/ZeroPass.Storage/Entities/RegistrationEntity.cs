using System;

namespace ZeroPass.Storage.Entities
{
    public enum ActivateType : int
    {
        Personal = 1,
        Business = 2
    }

    [Serializable]
    public class RegistrationEntity
    {
        public ActivateType ActivateType { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public object Raw { get; set; }
    }
}
