using System;


namespace Transversal.DTOs.Transactional
{
    public class ResponseCountingDto
    {
        public string Estacion
        {
            get; set;
        }

        public string Sentido
        {
            get; set;
        }

        public TimeSpan Hora
        {
            get; set;
        }

        public string Categoria
        {
            get; set;
        }

        public int Cantidad
        {
            get; set;
        }
    }
}
