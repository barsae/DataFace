using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NSubstitute;
using DataFace.Core;

namespace DataFace.Test.MultipleResultSetConverterTests {
    [TestClass]
    public class ToScalarTests {
        [TestMethod]
        public void ToScalar_IsValid_Works() {
            var resultSet = Substitute.For<IResultSet>();
            var columns = new List<Column>() { new Column("Value") };
            var row = Substitute.For<IRow>();

            resultSet.GetColumns()
                     .Returns(columns);
            resultSet.GetRows()
                     .Returns(new List<IRow>() { row });
            row.GetValues()
               .Returns(new List<object>() { 14 });
            
            var converter = new MultipleResultSetConverter(new List<IResultSet>() {
                resultSet
            });

            Assert.AreEqual(14, converter.ToScalar<int>());
        }
    }
}
