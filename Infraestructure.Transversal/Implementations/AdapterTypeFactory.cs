using Infraestructure.Transversal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Transversal.Implementations
{
    public class AdapterTypeFactory
    {
        #region Members

        static IAdapterTypeFactory _currentAdapterTypeFactory = null;
        static IAdapterType _adapterType = null;

        #endregion

        #region Static Methods

        public static void SetCurrent(IAdapterTypeFactory adapterFactory)
        {
            _currentAdapterTypeFactory = adapterFactory;
        }

        public static IAdapterType Create()
        {
            if (_currentAdapterTypeFactory == null)
                throw new ApplicationException(Resources.Messages.Exception_AdapterType);

            if (_adapterType == null)
                _adapterType = _currentAdapterTypeFactory.Create();

            return _adapterType;
        }

        #endregion

    }
}
