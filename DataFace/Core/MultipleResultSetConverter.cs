﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataFace.Core {
    public class MultipleResultSetConverter {
        public List<ResultSet> ResultSets { get; set; }

        private ResultSetConverter converter;

        public MultipleResultSetConverter(List<ResultSet> resultSets) {
            this.ResultSets = resultSets;
            converter = new ResultSetConverter();
        }

        public ScalarType ToScalar<ScalarType>() {
            return converter.ToScalar<ScalarType>(ResultSets.Single());
        }

        public ScalarType ToFirstOrDefaultScalar<ScalarType>() {
            return converter.ToFirstOrDefaultScalar<ScalarType>(ResultSets.Single());
        }

        public List<ScalarType> ToScalars<ScalarType>() {
            return converter.ToScalars<ScalarType>(ResultSets.Single());
        }

        public RowType ToSingleRow<RowType>() where RowType : new() {
            return converter.ToSingleRow<RowType>(ResultSets.Single());
        }

        public RowType ToSingleOrDefaultRow<RowType>() where RowType : new() {
            return converter.ToSingleOrDefaultRow<RowType>(ResultSets.Single());
        }

        public List<RowType> ToRows<RowType>() where RowType : new() {
            return converter.ToRows<RowType>(ResultSets.Single());
        }

        public MultipleResultSetModelType ToMultipleResultSetModel<MultipleResultSetModelType>() where MultipleResultSetModelType : new() {
            var model = new MultipleResultSetModelType();
            var properties = typeof(MultipleResultSetModelType).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties) {
                var attribute = property.GetCustomAttribute<ResultSetAttribute>();

                if (attribute != null) {
                    var resultSet = ResultSets[attribute.Index];
                    property.SetValue(model, ConvertResultSet(resultSet, attribute.Type, property.PropertyType));
                }
            }

            return model;
        }

        public List<Dictionary<string, string>> ToDictionary() {
            return converter.ToDictionary(ResultSets.Single());
        }

        private object ConvertResultSet(ResultSet resultSet, ResultSetType resultSetType, Type propertyType) {
            switch (resultSetType) {
                case ResultSetType.Scalar: return converter.ToScalar(propertyType, resultSet);
                case ResultSetType.Scalars: return converter.ToScalars(GetUnderlyingListType(propertyType), resultSet);
                case ResultSetType.SingleRow: return converter.ToSingleRow(propertyType, resultSet);
                case ResultSetType.SingleOrDefaultRow: return converter.ToSingleOrDefaultRow(propertyType, resultSet);
                case ResultSetType.Rows: return converter.ToRows(GetUnderlyingListType(propertyType), resultSet);
                default: throw new NotImplementedException();
            }
        }

        private Type GetUnderlyingListType(Type propertyType) {
            return propertyType.GetGenericArguments().First();
        }
    }
}
