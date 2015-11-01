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

        public ScalarType ToScalar<ScalarType>(IResultSet resultSet) {
            return (ScalarType)ToScalar(typeof(ScalarType), resultSet);
        }

        public object ToScalar(Type scalarType, IResultSet resultSet) {
            return resultSet.GetRows()
                            .Single()
                            .GetValues()
                            .Single();
        }

        public RowType ToSingleRow<RowType>(IResultSet resultSet) where RowType : new() {
            return (RowType)ToSingleRow(typeof(RowType), resultSet);
        }

        public object ToSingleRow(Type rowType, IResultSet resultSet) {
            return converter.ConvertRowToObject(rowType, resultSet.GetColumns(), resultSet.GetRows().Single());
        }

        public RowType ToSingleOrDefaultRow<RowType>(IResultSet resultSet) where RowType : new() {
            return (RowType)ToSingleOrDefaultRow(typeof(RowType), resultSet);
        }

        public object ToSingleOrDefaultRow(Type rowType, IResultSet resultSet) {
            return converter.ConvertRowToObject(rowType, resultSet.GetColumns(), resultSet.GetRows().SingleOrDefault());
        }

        public List<RowType> ToRows<RowType>(IResultSet resultSet) where RowType : new() {
            return (List<RowType>)ToRows(typeof(RowType), resultSet);
        }

        public object ToRows(Type rowType, IResultSet resultSet) {
            var binder = new RowConverter();
            var columns = resultSet.GetColumns();
            return resultSet.GetRows()
                            .Select(row => binder.ConvertRowToObject(rowType, columns, row))
                            .ToListOfDynamicType(rowType);
        }
    }
}
