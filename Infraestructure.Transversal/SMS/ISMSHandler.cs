using System.Threading.Tasks;

namespace Infraestructure.Transversal.SMS
{
    public interface ISMSHandler
    {
        SMSSendRequest _smsSendRequest { get; set; }

        Task<bool> SendMessage();
    }
}