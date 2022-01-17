using System;

namespace Transversal.DTOs.Transactional.Response
{
    public class ResponseVehicleCollectionDto
    {
        public string Estation
        {
            get; set;
        }

        public string Direction
        {
            get; set;
        }

        public TimeSpan Hour
        {
            get; set;
        }

        public string Category
        {
            get; set;
        }

        public int Amount
        {
            get; set;
        }

        public decimal TabulatedValue
        {
            get; set;
        }

        public DateTime CreatedDate
        {
            get; set;
        }
    }
}
