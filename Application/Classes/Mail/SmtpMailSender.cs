using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.Classes.Mail
{
    public class SmtpMailSender : ISmtpMailSender
    {
        private readonly SmtpSettings _settings;

        public SmtpMailSender(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<bool> SendEmail(IEnumerable<string> emails, IEnumerable<string> cc, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
                foreach (var email in emails)
                {
                    message.To.Add(new MailboxAddress(email, email));
                }

                if (cc != null)
                    foreach (var copy in cc)
                    {
                        message.Cc.Add(new MailboxAddress(copy, copy));
                    }

                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };
                using var client = new SmtpClient
                {
                    ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true
                };
                //if (Debugger.IsAttached) await client.ConnectAsync(Settings.Server, Settings.Port, true);
                //else 
                await client.ConnectAsync(_settings.Server);
                await client.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}