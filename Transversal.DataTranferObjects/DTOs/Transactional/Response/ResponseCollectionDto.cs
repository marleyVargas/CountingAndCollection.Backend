using System;
using System.Collections.Generic;

namespace Transversal.DTOs.Transactional
{
    public class CollectionDto
    {
        public string Estacion
        {
            get; set;
        }

        public string Sentido
        {
            get; set;
        }

        public int Hora
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

        public int Cantidad
        {
            get; set;
        }

        public DateTime Fecha
        {
            get; set;
        }
    }
}
