using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NSubstitute;
using DataFace.Test.MultipleResultSetConverterTests.TestModels;
using DataFace.Core;

namespace DataFace.Test.MultipleResultSetConverterTests {
    [TestClass]
    public class ToSingleOrDefaultRowTests {
        [TestMethod]
        public void ToSingleOrDefaultRow_SingleOrDefaultRow_Works() {
            var resultSet = new ResultSet() {
                Columns = new List<Column>() { new Column("Value") },
                Rows = new List<Row>() {
                    new Row(new List<object>() { 18 })
                }
            };
            
            var converter = new MultipleResultSetConverter(new List<ResultSet>() {
                resultSet
            });

            Assert.AreEqual(18, converter.ToSingleOrDefaultRow<ScalarTestModel>().Value);
        }
    }
}
