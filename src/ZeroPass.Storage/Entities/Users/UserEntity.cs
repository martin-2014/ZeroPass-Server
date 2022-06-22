namespace ZeroPass.Storage.Entities
{
    public class UserEntity : EntityBase
    {
        public int UserType { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }
}
