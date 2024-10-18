using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Cundi_incidencias.Dto;

namespace Cundi_incidencias.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void EnviarEmail(EmailDto emailDto)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(emailDto.Para));
            email.Subject = emailDto.Asunto;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = emailDto.Contenido
            };

            using var smtp = new SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls);

            smtp.Authenticate(_config.GetSection("Email:Username").Value,
                _config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
