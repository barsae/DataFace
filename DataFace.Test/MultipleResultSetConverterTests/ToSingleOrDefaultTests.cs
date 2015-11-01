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
            var resultSet = Substitute.For<IResultSet>();
            var columns = new List<Column>() { new Column("Value") };
            var row = Substitute.For<IRow>();

            resultSet.GetColumns()
                     .Returns(columns);
            resultSet.GetRows()
                     .Returns(new List<IRow>() { row });
            row.GetValues()
               .Returns(new List<object>() { 18 });
            
            var converter = new MultipleResultSetConverter(new List<IResultSet>() {
                resultSet
            });

            Assert.AreEqual(18, converter.ToSingleOrDefaultRow<ScalarTestModel>().Value);
        }
    }
}
