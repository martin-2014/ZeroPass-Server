using System.Threading;
using System.Threading.Tasks;

namespace ZeroPass.Service.Mediator
{
    internal partial class NotificationHandler
    {
        public  Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}