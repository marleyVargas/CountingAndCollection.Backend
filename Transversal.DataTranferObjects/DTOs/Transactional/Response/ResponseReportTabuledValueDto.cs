using System;

namespace Transversal.DTOs.Transactional.Response
{
    public class ResponseReportTabuledValueDto
    {
        public ResponseVehicleCollectionDto CollectionDto
        {
            get; set;
        }

        public int TotalAmount
        {
            get; set;
        }

        public decimal TotalTabulatedValue
        {
            get; set;
        }

       
    }
   
}
