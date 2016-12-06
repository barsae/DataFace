using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Core {
    public class RowConverter {
        public RowType ConvertRowToObject<RowType>(List<Column> columns, Row row) where RowType : new() {
            return (RowType)ConvertRowToObject(typeof(RowType), columns, row);
        }

        public object ConvertRowToObject(Type rowType, List<Column> columns, Row row) {
            var result = Activator.CreateInstance(rowType);
            var values = row.Values;

            for (int ii = 0; ii < columns.Count; ii++) {
                var value = values[ii];

                var propertyName = columns[ii].Name;
                var property = rowType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property != null) {
                    if (value != DBNull.Value) {
                        property.SetValue(result, ConvertValue(value, property.PropertyType));
                    }
                } else {
                    throw new DataFaceException(string.Format("Couldn't find property to bind to for column '{0}'", propertyName));
                }
            }

            return result;
        }

        public object ConvertValue(object value, Type type) {
            if (value == DBNull.Value) {
                if (type == typeof(string)) {
                    return (string)null;
                } else {
                    return Activator.CreateInstance(type);
                }
            } else {
                var unwrappedType = type;
                if (unwrappedType.IsGenericType && unwrappedType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                    unwrappedType = unwrappedType.GetGenericArguments()[0];
                }

                return Convert.ChangeType(value, unwrappedType);
            }
        }
    }
}
