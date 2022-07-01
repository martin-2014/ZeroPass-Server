namespace ZeroPass.Model.Models
{
    public interface IDomainActor
    {
        int UserId { get; }
        int DomainId { get; }
    }
}