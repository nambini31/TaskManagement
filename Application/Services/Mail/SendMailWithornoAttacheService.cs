using Application.Classes.Mail;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Mail
{
    public class SendMailWithornoAttacheService : ISendMailWithornoAttacheService
    {
        private readonly SmtpSettings _settings;

        public SendMailWithornoAttacheService(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public bool sendEmailWithAttache(IEnumerable<string> emails, List<string> cc, List<string> listpathattachement, string subject, string body)
        {
            var temptation = 0;
            while (temptation < 5)
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
                    {
                        foreach (var copy in cc)
                        {
                            message.Cc.Add(new MailboxAddress(copy, copy));
                        }
                    }

                    message.Subject = subject;

                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = body
                    };

                    if (listpathattachement != null)
                    {
                        foreach (var attachmentPath in listpathattachement)
                        {
                            if (File.Exists(attachmentPath))
                            {
                                bodyBuilder.Attachments.Add(attachmentPath);
                            }
                        }
                    }
                    message.Body = bodyBuilder.ToMessageBody();
                    using var client = new SmtpClient
                    {
                        ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true
                    };

                    client.Connect(_settings.Server, _settings.Port, false);
                    client.Authenticate(_settings.SenderEmail, _settings.Password);
                    client.Send(message);
                    client.Disconnect(true);

                    return true;
                }
                catch (Exception e)
                {
                    temptation++;
                }
            }

            return false;
        }


        public bool sendEmailWithOutAttache(IEnumerable<string> emails, List<string> cc, string subject, string body)
        {
            var temptation = 0;
            while (temptation < 5)
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
                    {
                        foreach (var copy in cc)
                        {
                            message.Cc.Add(new MailboxAddress(copy, copy));
                        }
                    }

                    message.Subject = subject;

                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = body
                    };

                    message.Body = bodyBuilder.ToMessageBody();
                    using var client = new SmtpClient
                    {
                        ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true
                    };

                    client.Connect(_settings.Server, _settings.Port, false);
                    client.Authenticate(_settings.SenderEmail, _settings.Password);
                    client.Send(message);
                    client.Disconnect(true);

                    return true;
                }
                catch (Exception e)
                {
                    temptation++;
                }
            }

            return false;
        }
    }
}
