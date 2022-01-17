using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Nucleus.Exceptions
{
    public class BusinessException : Exception
    {
        public int MyProperty
        {
            get; set;
        }

        public BusinessException()
        {

        }

        public BusinessException(string message) : base(message)
        {

        }

    }
}
