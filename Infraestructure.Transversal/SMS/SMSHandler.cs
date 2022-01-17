using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Transversal.SMS
{
    public class SMSHandler : ISMSHandler
    {
        public SMSSendRequest _smsSendRequest { get; set; }
        public SMSHandler(SMSSendRequest smsSendRequest)
        {
            _smsSendRequest = smsSendRequest;
        }

        public async Task<bool> SendMessage()
        {
            try
            {
                SMSFacadeService.SMSFacadeClient smsClient = new SMSFacadeService.SMSFacadeClient(SMSFacadeService.SMSFacadeClient.EndpointConfiguration.WSHttpBinding_ISMSFacade);

                smsClient.ClientCredentials.UserName.UserName = _smsSendRequest.Username;
                smsClient.ClientCredentials.UserName.Password = _smsSendRequest.Password;

                //----------------------------------------------------------------
                //Evitar error de certificado SSL (Eliminar esta función en versión de Producción)
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
                //----------------------------------------------------------------

                int messageType = 66;
                SMSFacadeService.DatosMensaje smsdata = new SMSFacadeService.DatosMensaje {
                    Mensaje = _smsSendRequest.Message,
                    NumeroDestino = _smsSendRequest.MobileNumber,
                    CodReferenciaCliente = DateTime.Now.ToString("yyyyMMddHHmmssFFF"),
                    FechaEnvio = DateTime.Now,
                    Url = _smsSendRequest.Url
                };
                SMSFacadeService.StatusMessage statusMessage = await smsClient.EnviarMensajeAsync(messageType, smsdata);

                if(statusMessage.Status == SMSFacadeService.Status.Ok)
                {
                    return true;
                }else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}
