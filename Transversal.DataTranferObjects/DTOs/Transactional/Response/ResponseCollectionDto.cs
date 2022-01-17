using System;
using System.Collections.Generic;

namespace Transversal.DTOs.Transactional
{
    public class ResponseCollectionDto
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

        public decimal ValorTabulado
        {
            get; set;
        }
    }
}
