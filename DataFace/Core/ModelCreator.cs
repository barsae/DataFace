using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFace.Core {
    public class ModelCreator<T> {
        public T CreateFromScalar(object scalar) {
            return (T)Convert.ChangeType(scalar, typeof(T));
        }

        public object CreateFromRow(Column column, Row row) {
            throw new NotImplementedException();
        }
    }
}
