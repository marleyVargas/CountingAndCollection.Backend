using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transversal.DTOs.Transactional.Request
{
    public class RequestFinalizeTransactionDto
    {
        public Guid GUID
        {
            get; set;
        }

        public long StatusId
        {
            get; set;
        }

        public int CustomerIndicatorId
        {
            get; set;
        }

        public string CodeAutorization
        {
            get; set;
        }

        public int Installments
        {
            get; set;
        }

        public decimal InstallmentsValue
        {
            get; set;
        }

    }
}
