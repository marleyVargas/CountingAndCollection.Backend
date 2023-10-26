using System;

namespace Transversal.DTOs
{
    public class Response<T> : IResponse<T>
    {
        public Response()
        {
            
        }
        public T Result { get; set; }

        public static implicit operator Response<T>(bool v)
        {
            throw new NotImplementedException();
        }
    }
}