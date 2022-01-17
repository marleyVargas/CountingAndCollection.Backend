using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Transversal.SMS
{
    public class SMSSendRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        public string MobileNumber { get; set; }
        public string Url { get; set; }
    }
}
