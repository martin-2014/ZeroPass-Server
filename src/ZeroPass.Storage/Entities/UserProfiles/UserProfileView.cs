namespace ZeroPass.Storage.Entities
{
    public class UserProfileView
    {
        public int Id { get; set; }

        public int UserType { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Timezone { get; set; }

    }
}