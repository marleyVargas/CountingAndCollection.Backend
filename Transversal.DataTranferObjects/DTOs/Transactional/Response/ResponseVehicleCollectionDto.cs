using Domain.Nucleus.CustomEntities;
using System;
using System.Collections.Generic;

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

    public class ResponseCollectionPaginatorDto
    {
        public List<ResponseVehicleCollectionDto> data
        {
            get; set;
        }

        public Metadata meta
        {
            get; set;
        }
       
    }
}
