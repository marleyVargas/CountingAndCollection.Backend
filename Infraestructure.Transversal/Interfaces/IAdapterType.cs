using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Transversal.Interfaces
{
    public interface IAdapterType
    {
        #region Métodos

        TTarget Adapter<TTarget>(object source);

        TTarget Adapter<TSource, TTarget>(TSource source);

        TTarget Adapter<TSource, TTarget>(TSource source, TTarget target);

        IEnumerable<TTarget> Adapter<TTarget>(IEnumerable<object> sources);

        IEnumerable<TTarget> Adapter<TSource, TTarget>(IEnumerable<TSource> sources);

        IEnumerable<TTarget> Adapter<TSource, TTarget>(IEnumerable<TSource> sources,
            IEnumerable<TTarget> targets);

        TTarget Adapter<TTarget>(object source, string llaveEncriptacion,
            bool cifradoEstatico = false);

        TTarget Adapter<TSource, TTarget>(TSource source, string llaveEncriptacion,
            bool cifradoEstatico = false);

        TTarget Adapter<TSource, TTarget>(TSource source, TTarget target, string llaveEncriptacion,
            bool cifradoEstatico = false);

        #endregion
    }
}
