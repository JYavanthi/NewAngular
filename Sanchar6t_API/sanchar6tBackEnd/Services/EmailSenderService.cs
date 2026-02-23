using System.Net;
using System.Net.Mail;

namespace sanchar6tBackEnd.Services
{
    public class EmailSenderService
    {
        private readonly IConfiguration _configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpSection = _configuration.GetSection("Smtp");

            var message = new MailMessage
            {
                From = new MailAddress(smtpSection["From"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            var smtp = new SmtpClient(smtpSection["Host"], int.Parse(smtpSection["Port"]))
            {
                Credentials = new NetworkCredential(
                    smtpSection["Username"],
                    smtpSection["Password"]
                ),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}
