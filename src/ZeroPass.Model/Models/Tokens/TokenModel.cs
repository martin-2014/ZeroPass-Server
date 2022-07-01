namespace ZeroPass.Model.Models
{
    public partial class TokenModel : IActor
    {
        public TokenModel(
            int userId,
            int personalDomainId)
            =>
                (PersonalDomainId, UserId) =
                (personalDomainId, userId);

        public TokenModel() { }

        public int UserId { get; set; }

        public int PersonalDomainId { get; set; }

        public int DomainId { get; set; }


        public IDomainActor Personal() => new TokenModel { UserId = UserId, DomainId = PersonalDomainId}; 
    }
}