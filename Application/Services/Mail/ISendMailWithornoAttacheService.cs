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
    public interface ISendMailWithornoAttacheService
    {
        public bool sendEmailWithAttache(IEnumerable<string> emails, List<string> cc, List<string> listpathattachement, string subject, string body);
        public bool sendEmailWithOutAttache(IEnumerable<string> emails, List<string> cc, string subject, string body);

    }
}
