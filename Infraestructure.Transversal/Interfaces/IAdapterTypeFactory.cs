using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Transversal.Interfaces
{
    public interface IAdapterTypeFactory
    {
        IAdapterType Create();
    }
}
