using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NSubstitute;
using DataFace.Test.MultipleResultSetConverterTests.TestModels;

namespace DataFace.Test.MultipleResultSetConverterTests {
    [TestClass]
    public class ToRowsTests {
        [TestMethod]
        public void ToRows_SingleRow_Works() {
            var resultSet = Substitute.For<IResultSet>();
            var columns = new List<Column>() { new Column("Value") };
            var row = Substitute.For<IRow>();

            resultSet.GetColumns()
                     .Returns(columns);
            resultSet.GetRows()
                     .Returns(new List<IRow>() { row });
            row.GetValues()
               .Returns(new List<object>() { 42 });
            
            var converter = new MultipleResultSetConverter(new List<IResultSet>() {
                resultSet
            });
            var model = converter.ToRows<ScalarTestModel>();

            Assert.AreEqual(1, model.Count);
            Assert.AreEqual(42, model[0].Value);
        }
    }
}
