using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transversal.DTOs.Transactional.Request
{
    public class RequestPaymentDto
    {
        public string Reference
        {
            get; set;
        }

        public string Concept
        {
            get; set;
        }

        public string AppKey
        {
            get; set;
        }

        public string Method
        {
            get; set;
        }

        public decimal TaxValue
        {
            get; set;
        }

        public decimal TotalValue
        {
            get; set;
        }

        public string Currency
        {
            get; set;
        }

        public string IpAddress
        {
            get; set;
        }

        public MiniCartDto MiniCart
        {
            get; set;
        }

        public string Url
        {
            get; set;
        }

        public string CallbackUrl
        {
            get; set;
        }

        public string ReturnUrl
        {
            get; set;
        }

        public string TransactionGuid
        {
            get; set;
        }

        public string ShowMode
        {
            get; set;
        }
}

    public class MiniCartDto
    {
        [Required]
        public BuyerDto Buyer
        {
            get; set;
        }

        public AddressDto Address
        {
            get; set;
        }

        public List<ItemDto> Items
        {
            get; set;
        }

    }

    public class BuyerDto
    {
        public string FirstName
        {
            get; set;
        }

        public string LastName
        {
            get; set;
        }

        public string DocumentNumber
        {
            get; set;
        }

        public string DocumentTypeCode
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
    }

    public class ItemDto
    {
        public string Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public decimal Price
        {
            get; set;
        }

        public decimal Quantity
        {
            get; set;
        }

        public decimal Discount
        {
            get; set;
        }

    }

    public class AddressDto
    {
        public string CountryCode
        {
            get; set;
        }

        public string City
        {
            get; set;
        }

        public string Neighborhood
        {
            get; set;
        }

        public string PostalCode
        {
            get; set;
        }

        public string Direction
        {
            get; set;
        }
    }


}
