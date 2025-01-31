using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Mail
{
    public interface ISendMailService
    {
        public bool sendMail(IEnumerable<string> destemails, string subject, string Content, bool withattache, List<string> cc, List<string> listpathattachement = null);

    }
}


