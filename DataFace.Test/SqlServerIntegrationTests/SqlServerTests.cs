﻿using DataFace.Core;
using DataFace.SqlServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.SqlServerIntegrationTests {
    [TestClass]
    public class SqlServerTests {
        [TestMethod]
        public void SqlServer_ToScalar_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new SqlServerIntegrationRepository(GetConnection());
            Assert.AreEqual(142, repo.ToScalar());
        }

        [TestMethod]
        public void SqlServer_MultipleResultSets_Work() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new SqlServerIntegrationRepository(GetConnection());
            var model = repo.ToMultipleResultSetModel();

            Assert.AreEqual(model.ResultSet0.Count, 2);
            Assert.AreEqual(model.ResultSet0[0].IntValue, 147);
            Assert.AreEqual(model.ResultSet0[1].IntValue, 192);
            Assert.AreEqual(model.ResultSet0[0].BoolValue, true);
            Assert.AreEqual(model.ResultSet0[1].BoolValue, false);

            Assert.AreEqual(model.ResultSet1.Count, 2);
            Assert.AreEqual(model.ResultSet1[0].StringValue, "abc");
            Assert.AreEqual(model.ResultSet1[1].StringValue, "def");
            Assert.AreEqual(model.ResultSet1[0].DateTimeValue, new DateTime(2001, 2, 3));
            Assert.AreEqual(model.ResultSet1[1].DateTimeValue, new DateTime(2004, 5, 6));

        }

        private IDatabaseConnection GetConnection() {
            var connectionString = "Server=localhost;Database=DataFaceIntegrationTests;Integrated Security=True;";
            if (connectionString != "Server=localhost;Database=DataFaceIntegrationTests;Integrated Security=True;") {
                throw new Exception("Do not change the integration test connection string");
            }
            return new SqlServerDatabaseConnection(connectionString);
        }

        private void InitializeDatabase(IDatabaseConnection connection) {
            using (var transaction = connection.BeginTransaction()) {
                foreach (var batch in ReadSqlFileIntoBatches("SqlServerIntegrationTests\\CreateTestDatabase.sql")) {
                    if (batch.Trim() != "") {
                        transaction.ExecuteAdHocQuery(batch);
                    }
                }
            }
        }

        private IEnumerable<string> ReadSqlFileIntoBatches(string filename) {
            var builder = new StringBuilder();
            foreach (var line in File.ReadAllLines(filename)) {
                if (line.Trim() == "GO") {
                    yield return builder.ToString();
                    builder.Clear();
                } else {
                    builder.AppendLine(line);
                }
            }
            yield return builder.ToString();
        }
    }
}
