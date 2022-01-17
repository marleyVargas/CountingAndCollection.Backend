using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Nucleus.Entities.Application
{
    public class LogRequestAndResponse : BaseEntity
    {
        public string Ip
        {
            get; set;
        }

        public string Method
        {
            get; set;
        }

        public string Identifier
        {
            get; set;
        }

        public string Request
        {
            get; set;
        }

        public string Response
        {
            get; set;
        }

        public DateTime RequestDate
        {
            get; set;
        }

        public DateTime ResponseDate
        {
            get; set;
        }

        public string ExceptionType
        {
            get; set;
        }

        public string ExceptionMessage
        {
            get; set;
        }

        public string ExceptionStackTrace
        {
            get; set;
        }
    }
}
