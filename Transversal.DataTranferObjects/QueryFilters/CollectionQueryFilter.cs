using System;

namespace Transversal.QueryFilters
{
    public class CollectionQueryFilter
    {
        public DateTime? CreatedDateInit
        {
            get; set;
        }

        public DateTime? CreatedDateFin
        {
            get; set;
        }

        public int PageSize
        {
            get; set;
        }

        public int PageNumber
        {
            get; set;
        }
    }
}
