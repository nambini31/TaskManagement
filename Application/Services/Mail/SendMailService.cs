using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Mail
{
    public class SendMailService : ISendMailService
    {
        private readonly ISendMailWithornoAttacheService _SendMailAttache;
        public SendMailService(ISendMailWithornoAttacheService sendMailWithAttache)
        {
            _SendMailAttache = sendMailWithAttache;
        }

        public bool sendMail(IEnumerable<string> destemails, string subject, string Content, bool withattache, List<string> cc, List<string> listpathattachement = null)
        {
            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (sender, e) =>
                {
                    if (withattache)
                    {
                        _SendMailAttache.sendEmailWithAttache(destemails, cc, listpathattachement, subject, Content);
                    }
                    else
                    {
                        _SendMailAttache.sendEmailWithOutAttache(destemails, cc, subject, Content);
                    }
                };
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                //ignore 
            }
            return false;
        }
    }
}


