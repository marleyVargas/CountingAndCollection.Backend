using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Nucleus.Entities.Orchestrator
{
    public class VehicleCountingResponse
    {
        public string Estacion
        {
            get; set;
        }

        public string Sentido
        {
            get; set;
        }

        public string Hora
        {
            get; set;
        }

        public string Categoria
        {
            get; set;
        }

        public string Cantidad
        {
            get; set;
        }

    }
}
