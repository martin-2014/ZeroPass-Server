using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using ZeroPass.Model;
using ZeroPass.Model.Configuration;
using ZeroPass.Service.Properties;

namespace ZeroPass.Service
{
    internal class EmailService : IEmailService
    {
        IConfiguration Configuration;

        public EmailService(IConfiguration configuration) => Configuration = configuration;

        public async Task Send(IList<string> recipients, string subject, string body)
        {
            var sender = Configuration.GetValue("SMTP_USER");
            using var client = new SmtpClient(Configuration.GetValue("SMTP_SERVER"))
            {
                Credentials = new System.Net.NetworkCredential(
                    sender,
                    Configuration.GetValue("SMTP_PASSWORD")),
            };

            var fromMail = new MailAddress(sender, Resources.Email_Sender_Name);
            using var message = new MailMessage(sender, string.Join(',', recipients))
            {
                From = fromMail,
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            await client.SendMailAsync(message);
        }
    }
}
