using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Infraestructure.Transversal.Mail
{
    public class MailHandler : IMailHandler
    {
        public MailServer _mailServer { get; set; }
        public MailHandler(MailServer mailServer)
        {
            _mailServer = mailServer;
        }
        public async Task<MailSendResponse> SendMail(ICollection<string> receivers,
            string subject, string htmlMessage, AlternateView htmlView,
            IEnumerable<Attachment> attachments = null)
        {
            MailSendResponse response = new MailSendResponse();
            response.Result = false;
            try
            {
                MailAddress objMailFrom = new MailAddress(_mailServer.FromAddress, _mailServer.FromName);
                //MailAddress objCorreoPara = new MailAddress(destinatarios.ToList()[0], nombreDestinatario);
                MailAddress objMailTo = new MailAddress(receivers.ToList()[0]);
                MailMessage objMailMessage = new MailMessage(objMailFrom, objMailTo);
                objMailMessage.Subject = subject;
                objMailMessage.Body = htmlMessage;

                objMailMessage.IsBodyHtml = true;
                if (htmlView != null)
                    objMailMessage.AlternateViews.Add(htmlView);

                if (string.IsNullOrEmpty(_mailServer.FromAddress))
                    throw new ArgumentNullException(nameof(_mailServer.FromAddress));

                if (string.IsNullOrEmpty(_mailServer.FromName))
                    throw new ArgumentNullException(nameof(_mailServer.FromName));

                if (string.IsNullOrEmpty(subject))
                    throw new ArgumentNullException(nameof(subject));

                if (string.IsNullOrEmpty(htmlMessage))
                    throw new ArgumentNullException(nameof(htmlMessage));

                if (receivers.Count() == 0)
                    throw new ArgumentNullException(nameof(receivers));

                //if (usarBCC)
                //{
                //    destinatarios.ToList().Skip(1).ToList().ForEach(d =>
                //    {
                //        objMailMessage.Bcc.Add(d);
                //    });
                //}
                //else
                //{
                receivers.ToList().Skip(1).ToList().ForEach(d =>
                {
                    objMailMessage.To.Add(d);
                });
                //}

                if (attachments != null)
                {
                    foreach (var s in attachments)
                    {
                        objMailMessage.Attachments.Add(s);
                    }
                }

                SmtpClient servidor = new SmtpClient();
                servidor.Host = _mailServer.Host;

                if (!string.IsNullOrEmpty(_mailServer.Port))
                    servidor.Port = Convert.ToInt32(_mailServer.Port);

                if (!string.IsNullOrEmpty(_mailServer.Password))
                {
                    var basicCredential = new NetworkCredential(_mailServer.Username, _mailServer.Password);
                    servidor.UseDefaultCredentials = true;
                    servidor.Credentials = basicCredential;
                    servidor.EnableSsl = true;
                }

                servidor.SendCompleted += (s, e) =>
                {
                    servidor.Dispose();
                    objMailMessage.Dispose();
                };

                await servidor.SendMailAsync(objMailMessage);

                response.Result = true;
                return response;
            }
            catch (ArgumentNullException)
            {
                response.Result = false;
                return response;
            }
            catch (Exception exception)
            {
                response.Result = false;
                response.Message = exception.Message;
                return response;
            }
        }

    }
}
