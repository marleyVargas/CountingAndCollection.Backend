using System;

namespace Transversal.DTOs.Transactional.Response
{
    public class ResponseVehicleCollectionDto
    {
        public string Station
        {
            get; set;
        }

        public string Direction
        {
            get; set;
        }

        public decimal Hour
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

        public DateTime QueryDate
        {
            get; set;
        }
    }
}
