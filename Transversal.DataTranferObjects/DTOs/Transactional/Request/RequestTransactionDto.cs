using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transversal.DTOs.Transactional.Request
{
    public class RequestTransactionDto
    {
        public long Id
        {
            get; set;
        }

        public long MerchantProductId
        {
            get; set;
        }

        public long BuyerId
        {
            get; set;
        }

        public long? PayerId
        {
            get; set;
        }

        public long PaymentId
        {
            get; set;
        }

        public long? MiniCartId
        {
            get; set;
        }

        public long StatusId
        {
            get; set;
        }

        public string StatusDescription
        {
            get; set;
        }

        public string CodeAutorization
        {
            get; set;
        }

        public string IPAddress
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

        public decimal TaxtValue
        {
            get; set;
        }

        public decimal TotalValue
        {
            get; set;
        }

        public string Email
        {
            get; set;
        }

        public string Phone
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
