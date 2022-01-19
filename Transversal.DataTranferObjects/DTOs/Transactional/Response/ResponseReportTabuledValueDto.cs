using System;
using System.Collections.Generic;

namespace Transversal.DTOs.Transactional.Response
{
    public class ResponseReportTabuledValueDto
    {
        public ReportStation ReportStation
        {
            get; set;
        }

        public int TotalsAmount
        {
            get; set;
        }

        public decimal TotalsTabulatedValue
        {
            get; set;
        }
    }

    public class ReportStation
    {
        public List<DataStation> DataStation
        {
            get; set;
        }

        public int TotalAmount
        {
            get; set;
        }

        public decimal TotalValue
        {
            get; set;
        }
    }


    public class DataStation
    {
        public string Date
        {
            get; set;
        }

        public List<StationDto> StationDto
        {
            get; set;
        }
    }

    public class StationDto
    {
        public string Station
        {
            get; set;
        }

        public int TotalQuantity
        {
            get; set;
        }

        public decimal TotalValue
        {
            get; set;
        }
    }

}
