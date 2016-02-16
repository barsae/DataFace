using DataFace.Core;
using DataFace.MySql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.MySqlIntegrationTests {
    [TestClass]
    public class MySqlTests {
        [TestMethod]
        public void MySql_ToScalar_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new MySqlIntegrationRepository(GetConnection());
            Assert.AreEqual(142, repo.ToScalar());
        }

        [TestMethod]
        public void MySql_ToScalars_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new MySqlIntegrationRepository(GetConnection());
            var result = repo.ToScalars();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(142, result[0]);
            Assert.AreEqual(143, result[1]);
            Assert.AreEqual(144, result[2]);
        }

        [TestMethod]
        public void MySql_InsertRecord_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new MySqlIntegrationRepository(GetConnection());
            var result = repo.InsertRecord();

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void MySql_MultipleResultSets_Work() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new MySqlIntegrationRepository(GetConnection());
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

        [TestMethod]
        public void MySql_SprocWithParameter_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new MySqlIntegrationRepository(GetConnection());
            Assert.AreEqual(143, repo.SprocWithParameter(143));
        }

        [TestMethod]
        public void MySql_CommitTransaction_HasSideEffect() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new MySqlIntegrationRepository(GetConnection());
            using (var transaction = repo.WithTransaction()) {
                repo.SprocWithSideEffect();
                transaction.Commit();
            }

            Assert.AreEqual(1, repo.GetCountOfSideEffects());
        }

        [TestMethod]
        public void MySql_RollbackTransaction_DoesntHaveSideEffect() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new MySqlIntegrationRepository(GetConnection());
            using (var transaction = repo.WithTransaction()) {
                repo.SprocWithSideEffect();
                transaction.Rollback();
            }

            Assert.AreEqual(0, repo.GetCountOfSideEffects());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MySql_SprocWithSchema_ThrowsException() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new MySqlIntegrationRepository(GetConnection());
            repo.SprocWithSchema();
        }

        private IDatabaseConnection GetConnection() {
            var connectionString = "Server=localhost;Database=DataFaceIntegrationTests;Username=dataface;Password=datafacetest";
            if (connectionString != "Server=localhost;Database=DataFaceIntegrationTests;Username=dataface;Password=datafacetest") {
                throw new Exception("Do not change the integration test connection string");
            }
            return new MySqlDatabaseConnection(connectionString);
        }

        private void InitializeDatabase(IDatabaseConnection connection) {
            using (var transaction = connection.BeginTransaction()) {
                foreach (var batch in ReadSqlFileIntoBatches("MySqlIntegrationTests\\CreateTestDatabase.sql")) {
                    if (batch.Trim() != "") {
                        transaction.ExecuteAdHocQuery(batch);
                    }
                }

                transaction.Commit();
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
