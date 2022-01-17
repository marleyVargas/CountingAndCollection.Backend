using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transversal.DTOs.Application
{
    public class ParameterDto
    {
        public long Id
        {
            get; set;
        }

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

        public DateTime CreatedDate
        {
            get; set;
        }

        public DateTime? UpdatedDate
        {
            get; set;
        }

        public bool Active
        {
            get; set;
        }

        public bool IsDeleted
        {
            get; set;
        }
    }
}
