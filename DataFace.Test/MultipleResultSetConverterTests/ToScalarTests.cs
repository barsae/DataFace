using System;
using NUnit.Framework;
using System.Collections.Generic;
using NSubstitute;
using DataFace.Core;

namespace DataFace.Test.MultipleResultSetConverterTests {
    [TestFixture]
    public class ToScalarTests {
        [TestCase]
        public void ToScalar_IsValid_Works() {
            var resultSet = new ResultSet() {
                Columns = new List<Column>() { new Column("Value") },
                Rows = new List<Row>() {
                    new Row(new List<object>() { 14 })
                }
            };
            
            var converter = new MultipleResultSetConverter(new List<ResultSet>() {
                resultSet
            });

            Assert.AreEqual(14, converter.ToScalar<int>());
        }
    }
}
