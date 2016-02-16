﻿using System;
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
                        SetValue(result, value, property);
                    }
                } else {
                    throw new DataFaceException(string.Format("Couldn't find property to bind to for column '{0}'", propertyName));
                }
            }

            return result;
        }

        private void SetValue(object model, object value, PropertyInfo property) {
            var unwrappedType = property.PropertyType;
            if (unwrappedType.IsGenericType && unwrappedType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                unwrappedType = unwrappedType.GetGenericArguments()[0];
            }

            property.SetValue(model, Convert.ChangeType(value, unwrappedType));
        }
    }
}
