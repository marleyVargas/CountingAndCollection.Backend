using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Nucleus.Entities.Application
{
    public class Parameters : BaseEntity
    {
        public string Code
        {
            get; set;
        }

        public string Categories
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public string Value
        {
            get; set;
        }
    }
}
