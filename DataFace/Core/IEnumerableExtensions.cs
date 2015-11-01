using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Core {
    public static class IEnumerableExtensions {
        public static object ToListOfDynamicType<T>(this IEnumerable<T> sequence, Type type) {
            var castMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(type);
            var toListMethod = typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(type);
            var casted = castMethod.Invoke(null, new object[] { sequence });
            var toListed = toListMethod.Invoke(null, new object[] { casted });
            return toListed;
        }
    }
}
