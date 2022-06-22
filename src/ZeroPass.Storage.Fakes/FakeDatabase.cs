using System.Collections.Generic;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public class FakeDatabase
    {
        int GlobalUserId = 1;
        public readonly List<UserEntity> Users = new List<UserEntity>();
        public int AllocateUserId()
            => GlobalUserId++;
    }
}
