using MediatR;

namespace ZeroPass.Service.Mediator
{
    public class UserRegisteredEvent : INotification
    {
        public UserRegisteredEvent(int userId, string email)
        {
            UserId = userId;
            Email = email;
        }

        public int UserId { get; }
        public string Email { get; }
    }
}
