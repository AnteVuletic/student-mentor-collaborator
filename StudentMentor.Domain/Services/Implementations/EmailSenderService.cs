using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using StudentMentor.Domain.Models.Configurations;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Resources;
using StudentMentor.Domain.Services.Interfaces;

namespace StudentMentor.Domain.Services.Implementations
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly ILogger<EmailSenderService> _emailSenderLogger;
        private readonly EmailConfiguration _emailConfiguration;

        public EmailSenderService(
            IOptions<EmailConfiguration> emailOptions,
            ILogger<EmailSenderService> emailSenderLogger
        ) {
            _emailSenderLogger = emailSenderLogger;
            _emailConfiguration = emailOptions.Value;
        }

        public async Task SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            await Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var bodyBuilder = new BodyBuilder { HtmlBody = message.Content };
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password);

              await client.SendAsync(mailMessage);
            }
            catch(Exception e)
            {
                _emailSenderLogger.LogError(e, string.Format(ValidationMessages.EmailExceptionLog, mailMessage.To, mailMessage.HtmlBody));
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
