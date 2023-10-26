using Domain.Nucleus.CustomEntities;

namespace Transversal.Response
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            this.data = data;
        }

        public T data
        {
            get; set;
        }

        public Metadata Meta
        {
            get; set;
        }
    }



}
