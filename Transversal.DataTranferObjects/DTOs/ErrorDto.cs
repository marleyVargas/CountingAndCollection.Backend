using System.Collections.Generic;

namespace Transversal.DTOs
{
    public class ErrorDto
    {
        public IEnumerable<Errors> Errors
        {
            get; set;
        }
    }

    public class Errors
    {
        public int Status
        {
            get; set;
        }

        public string Title
        {
            get; set;
        }

        public string Detail
        {
            get; set;
        }
    }

}
