using System;

namespace Transversal.DTOs.Orchestrator.Login
{
    public class ResponseLoginDto
    {
        public string Token
        {
            get; set;
        }

        public DateTime Expiration
        {
            get; set;
        }
    }
}
