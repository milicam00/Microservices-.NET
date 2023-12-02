using System.Net;
using System.Net.Mail;

namespace BuildingBlocks.Application.Emails
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public SmtpEmailService(string smtpServer, int smtpPort, bool enableSsl, string smtpUsername, string smtpPassword/*IConfiguration configuration*/)
        {
            _smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                EnableSsl = enableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword)
            };


        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {

            var mailAddress = "milicam000@outlook.com";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mailAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(to);
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
