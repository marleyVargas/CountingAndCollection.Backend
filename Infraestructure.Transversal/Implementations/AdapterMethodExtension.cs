using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Transversal.Implementations
{
    public static class AdapterMethodExtension
    {
        public static TTarget Adapter<TSource, TTarget>(this TSource source)
        {
            return AdapterTypeFactory.Create().Adapter<TSource, TTarget>(source);
        }
        public static TTarget Adapter<TTarget>(this object source)
        {
            return AdapterTypeFactory.Create().Adapter<TTarget>(source);
        }
        public static IEnumerable<TTarget> Adapter<TTarget>(this IEnumerable<object> source)
        {
            return AdapterTypeFactory.Create().Adapter<TTarget>(source);
        }
    }
}
