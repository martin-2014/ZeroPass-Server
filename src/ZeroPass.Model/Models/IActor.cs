namespace ZeroPass.Model.Models
{
    public partial interface IActor : IDomainActor
    {
        int PersonalDomainId { get; set; }
    }
}