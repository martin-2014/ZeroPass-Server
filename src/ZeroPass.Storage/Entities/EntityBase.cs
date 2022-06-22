using System;

namespace ZeroPass.Storage.Entities
{
    [Serializable]
    public abstract class EntityBase
    {
        public int Id { get; set; }
    }
}
