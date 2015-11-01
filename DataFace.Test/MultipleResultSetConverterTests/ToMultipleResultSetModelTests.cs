using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NSubstitute;
using DataFace.Test.MultipleResultSetConverterTests.TestModels;
using DataFace.Core;

namespace DataFace.Test.MultipleResultSetConverterTests {
    [TestClass]
    public class ToMultipleResultSetModelTests {
        [TestMethod]
        public void ToMultipleResultSetModel_ToScalar_Works() {
            var resultSet = new ResultSet() {
                Columns = new List<Column>() { new Column("Value") },
                Rows = new List<Row>() {
                    new Row(new List<object>() { 21 })
                }
            };

            var converter = new MultipleResultSetConverter(new List<ResultSet>() {
                resultSet
            });
            var model = converter.ToMultipleResultSetModel<MultipleResultSet_Scalar_TestModel>();

            Assert.AreEqual(21, model.Value);
        }

        [TestMethod]
        public void ToMultipleResultSetModel_ToSingleRow_Works() {
            var resultSet = new ResultSet() {
                Columns = new List<Column>() { new Column("Value") },
                Rows = new List<Row>() {
                    new Row(new List<object>() { 22 })
                }
            };

            var converter = new MultipleResultSetConverter(new List<ResultSet>() {
                resultSet
            });
            var model = converter.ToMultipleResultSetModel<MultipleResultSet_SingleRow_TestModel>();

            Assert.AreEqual(22, model.SingleRow.Value);
        }

        [TestMethod]
        public void ToMultipleResultSetModel_ToSingleOrDefaultRow_Works() {
            var resultSet = new ResultSet() {
                Columns = new List<Column>() { new Column("Value") },
                Rows = new List<Row>() {
                    new Row(new List<object>() { 23 })
                }
            };

            var converter = new MultipleResultSetConverter(new List<ResultSet>() {
                resultSet
            });
            var model = converter.ToMultipleResultSetModel<MultipleResultSet_SingleOrDefaultRow_TestModel>();

            Assert.AreEqual(23, model.SingleOrDefaultRow.Value);
        }

        [TestMethod]
        public void ToMultipleResultSetModel_ToRows_Works() {
            var resultSet = new ResultSet() {
                Columns = new List<Column>() { new Column("Value") },
                Rows = new List<Row>() {
                    new Row(new List<object>() { 24 })
                }
            };

            var converter = new MultipleResultSetConverter(new List<ResultSet>() {
                resultSet
            });
            var model = converter.ToMultipleResultSetModel<MultipleResultSet_Rows_TestModel>();

            Assert.AreEqual(1, model.Rows.Count);
            Assert.AreEqual(24, model.Rows[0].Value);
        }
    }
}
