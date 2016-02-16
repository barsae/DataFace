using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NSubstitute;
using DataFace.Test.MultipleResultSetConverterTests.TestModels;
using DataFace.Core;

namespace DataFace.Test.MultipleResultSetConverterTests {
    [TestClass]
    public class RowConverterTests {
        [TestMethod]
        public void RowConverter_HandlesDBNull_WithoutCrashing() {
            var columns = new List<Column>() {
                new Column("Value")
            };
            var row = new Row(new List<object>() { DBNull.Value });
            
            var converter = new RowConverter();

            Assert.AreEqual(null, converter.ConvertRowToObject<NullableTestModel>(columns, row).Value);
        }

        [TestMethod]
        public void RowConverter_HandlesValue_Correctly() {
            var columns = new List<Column>() {
                new Column("Value")
            };
            var row = new Row(new List<object>() { "|" });
            
            var converter = new RowConverter();

            Assert.AreEqual('|', converter.ConvertRowToObject<NullableTestModel>(columns, row).Value);
        }
    }
}
