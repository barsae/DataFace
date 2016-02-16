using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NSubstitute;
using DataFace.Core;

namespace DataFace.Test.MultipleResultSetConverterTests {
    [TestClass]
    public class ToScalarsTests {
        [TestMethod]
        public void ToScalars_IsValid_Works() {
            var resultSet = new ResultSet() {
                Columns = new List<Column>() { new Column("Value") },
                Rows = new List<Row>() {
                    new Row(new List<object>() { 13 }),
                    new Row(new List<object>() { 14 }),
                    new Row(new List<object>() { 15 })
                }
            };
            
            var converter = new MultipleResultSetConverter(new List<ResultSet>() {
                resultSet
            });

            var results = converter.ToScalars<int>();
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual(13, results[0]);
            Assert.AreEqual(14, results[1]);
            Assert.AreEqual(15, results[2]);
        }
    }
}
