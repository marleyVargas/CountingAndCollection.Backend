using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Nucleus.Entities.Transactional
{
    public class Collection : BaseEntity
    {
        public string Station
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

    }
}
