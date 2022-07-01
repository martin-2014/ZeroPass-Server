using System;

namespace ZeroPass.Storage.Entities
{
    [Serializable]
    public class UserKeyDistributionEntity
    {
        public int AssigneeId { get; set; }

        public int AssignerId { get; set; }

        public string AssignerPrivateDataKey { get; set; }
    }
}