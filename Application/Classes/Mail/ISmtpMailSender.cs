using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Classes.Mail
{
    public interface ISmtpMailSender
    {
        Task<bool> SendEmail(IEnumerable<string> emails, IEnumerable<string> cc, string subject, string body);
    }
}