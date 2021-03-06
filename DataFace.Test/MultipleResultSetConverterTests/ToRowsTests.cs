﻿using System;
using NUnit.Framework;
using System.Collections.Generic;
using NSubstitute;
using DataFace.Test.MultipleResultSetConverterTests.TestModels;
using DataFace.Core;

namespace DataFace.Test.MultipleResultSetConverterTests {
    [TestFixture]
    public class ToRowsTests {
        [TestCase]
        public void ToRows_SingleRow_Works() {
            var resultSet = new ResultSet() {
                Columns = new List<Column>() { new Column("Value") },
                Rows = new List<Row>() {
                    new Row(new List<object>() { 42 })
                }
            };
            
            var converter = new MultipleResultSetConverter(new List<ResultSet>() {
                resultSet
            });
            var model = converter.ToRows<ScalarTestModel>();

            Assert.AreEqual(1, model.Count);
            Assert.AreEqual(42, model[0].Value);
        }
    }
}
