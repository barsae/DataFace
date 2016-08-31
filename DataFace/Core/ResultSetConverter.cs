using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataFace.Core {
    public class ResultSetConverter {
        private RowConverter converter;

        public ResultSetConverter() {
            converter = new RowConverter();
        }

        public ScalarType ToScalar<ScalarType>(ResultSet resultSet) {
            return (ScalarType)ToScalar(typeof(ScalarType), resultSet);
        }

        public object ToScalar(Type scalarType, ResultSet resultSet) {
            var value = resultSet.Rows
                                 .Single()
                                 .Values
                                 .Single();
            return converter.ConvertValue(value, scalarType);
        }

        public ScalarType ToFirstOrDefaultScalar<ScalarType>(ResultSet resultSet) {
            return (ScalarType)ToFirstOrDefaultScalar(typeof(ScalarType), resultSet);
        }


        public object ToFirstOrDefaultScalar(Type scalarType, ResultSet resultSet) {
            var row = resultSet.Rows.FirstOrDefault();

            if (row != null) {
                var value = row.Values.Single();
                return converter.ConvertValue(value, scalarType);
            }

            return null;
        }


        public List<ScalarType> ToScalars<ScalarType>(ResultSet resultSet) {
            return (List<ScalarType>)ToScalars(typeof(ScalarType), resultSet);
        }

        public object ToScalars(Type scalarType, ResultSet resultSet) {
            return resultSet.Rows
                            .Select(row => row.Values.Single())
                            .ToListOfDynamicType(scalarType);
        }

        public RowType ToSingleRow<RowType>(ResultSet resultSet) where RowType : new() {
            return (RowType)ToSingleRow(typeof(RowType), resultSet);
        }

        public object ToSingleRow(Type rowType, ResultSet resultSet) {
            return converter.ConvertRowToObject(rowType, resultSet.Columns, resultSet.Rows.Single());
        }

        public RowType ToSingleOrDefaultRow<RowType>(ResultSet resultSet) where RowType : new() {
            return (RowType)ToSingleOrDefaultRow(typeof(RowType), resultSet);
        }

        public object ToSingleOrDefaultRow(Type rowType, ResultSet resultSet) {
            var row = resultSet.Rows.SingleOrDefault();
            if (row == null) {
                return null;
            }
            return converter.ConvertRowToObject(rowType, resultSet.Columns, row);
        }

        public List<RowType> ToRows<RowType>(ResultSet resultSet) where RowType : new() {
            return (List<RowType>)ToRows(typeof(RowType), resultSet);
        }

        public object ToRows(Type rowType, ResultSet resultSet) {
            var binder = new RowConverter();
            var columns = resultSet.Columns;
            return resultSet.Rows
                            .Select(row => binder.ConvertRowToObject(rowType, columns, row))
                            .ToListOfDynamicType(rowType);
        }

        public List<Dictionary<string, string>> ToDictionary(ResultSet resultSet) {
            var results = new List<Dictionary<string, string>>();

            foreach (var row in resultSet.Rows) {
                var result = new Dictionary<string, string>();

                for (int ii = 0; ii < resultSet.Columns.Count; ii++) {
                    var column = resultSet.Columns[ii];
                    var value = row.Values[ii];
                    result[column.Name] = value == DBNull.Value ? null : value.ToString();
                }

                results.Add(result);
            }

            return results;
        }
    }
}
