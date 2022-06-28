using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZeroPass.Model
{
    public interface IEmailService
    {
        Task Send(IList<string> recipients, string subject, string body);
    }
}
