using Infraestructure.Transversal.Logs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Nucleus.Observers
{
    public abstract class Observer
    {
        public abstract void NotifyEvent(Information persistedInformation);

        public abstract void NotifyException(ErrorException errorModel);
    }
}
