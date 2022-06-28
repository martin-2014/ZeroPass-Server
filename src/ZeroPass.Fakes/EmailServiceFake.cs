using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroPass.Model;

namespace ZeroPass.Fakes
{
    public class EmailServiceFake : IEmailService
    {
        public List<string> Recipients { get; set; } = new List<string>();


        public Task Send(IList<string> recipients, string subject, string body)
        {
            Recipients.AddRange(recipients);
            return Task.CompletedTask;
        }
    }
}
