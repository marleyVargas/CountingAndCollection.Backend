using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transversal.DTOs.Account
{
    public class ResponseSecurityDto
    {
        public bool IsAuthenticated
        {
            get; set;
        }
        public string Token
        {
            get; set;
        }
        public object RegisteredUser
        {
            get; set;
        }
        public string Rol
        {
            get; set;
        }
    }
}
