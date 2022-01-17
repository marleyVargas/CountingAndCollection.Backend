using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infraestructure.Transversal.Mail
{
    public interface IMailHandler
    {
        MailServer _mailServer { get; set; }

        Task<MailSendResponse> SendMail(ICollection<string> receivers, string subject, string htmlMessage, AlternateView htmlView, IEnumerable<Attachment> attachments = null);
    }
}