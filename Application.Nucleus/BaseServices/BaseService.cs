using Application.Nucleus.Observers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Nucleus.BaseServices
{
    public class BaseService : Observable, IDisposable
    {

        #region Miembros

        private IList<IDisposable> _services;

        #endregion

        public BaseService(params IDisposable[] services)
        {

            if (services != null)
            {
                this._services = services;
            }

        }
        public string GetCurrentMethodName()
        {
            return new StackTrace(1).GetFrame(0).GetMethod().Name;
        }

        #region Miembros IDisposable

        public void Dispose()
        {
            if (_services != null)
            {
                foreach (var servicio in this._services)
                {
                    servicio.Dispose();
                }
                
                this._services = null;
            }
        }

        #endregion
    }
}
