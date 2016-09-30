using DataFace.Core;
using DataFace.PostgreSql;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.PostgreSqlIntegrationTests {
    [TestFixture]
    public class PostgreSqlTests {
        [TestCase]
        public void PostgreSql_ToScalar_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new PostgreSqlIntegrationRepository(GetConnection());
            Assert.AreEqual(142, repo.ToScalar());
        }

        //[TestCase]
        //public void PostgreSql_MultipleResultSets_Work() {
        //    IDatabaseConnection connection = GetConnection();
        //    InitializeDatabase(connection);

        //    var repo = new PostgreSqlIntegrationRepository(GetConnection());
        //    var model = repo.ToMultipleResultSetModel();

        //    Assert.AreEqual(model.ResultSet0.Count, 2);
        //    Assert.AreEqual(model.ResultSet0[0].IntValue, 147);
        //    Assert.AreEqual(model.ResultSet0[1].IntValue, 192);
        //    Assert.AreEqual(model.ResultSet0[0].BoolValue, true);
        //    Assert.AreEqual(model.ResultSet0[1].BoolValue, false);

        //    Assert.AreEqual(model.ResultSet1.Count, 2);
        //    Assert.AreEqual(model.ResultSet1[0].StringValue, "abc");
        //    Assert.AreEqual(model.ResultSet1[1].StringValue, "def");
        //    Assert.AreEqual(model.ResultSet1[0].DateTimeValue, new DateTime(2001, 2, 3));
        //    Assert.AreEqual(model.ResultSet1[1].DateTimeValue, new DateTime(2004, 5, 6));
        //}

        [TestCase]
        public void PostgreSql_SprocWithParameter_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new PostgreSqlIntegrationRepository(GetConnection());
            Assert.AreEqual(143, repo.SprocWithParameter(143));
        }

        [TestCase]
        public void PostgreSql_CommitTransaction_HasSideEffect() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new PostgreSqlIntegrationRepository(GetConnection());
            using (var transaction = repo.WithTransaction()) {
                repo.SprocWithSideEffect();
                transaction.Commit();
            }

            Assert.AreEqual(1, repo.GetCountOfSideEffects());
        }

        [TestCase]
        public void PostgreSql_RollbackTransaction_DoesntHaveSideEffect() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new PostgreSqlIntegrationRepository(GetConnection());
            using (var transaction = repo.WithTransaction()) {
                repo.SprocWithSideEffect();
                transaction.Rollback();
            }

            Assert.AreEqual(0, repo.GetCountOfSideEffects());
        }

        [TestCase]
        public void PostgreSql_SprocWithSchema_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new PostgreSqlIntegrationRepository(GetConnection());
            Assert.AreEqual(123, repo.SprocWithSchema());
        }

        [TestCase]
        public void PostgreSql_ToSingleOrDefault_Works() {
            IDatabaseConnection connection = GetConnection();

            var repo = new PostgreSqlIntegrationRepository(GetConnection());
            repo.ToSingleOrDefault();
        }

        [TestCase]
        public void PostgreSql_ManyQueries_DoesntFail() {
            IDatabaseConnection connection = GetConnection();

            var repo = new PostgreSqlIntegrationRepository(GetConnection());

            for (int ii = 0; ii < 100; ii++) {
                repo.ExecuteAdHocQuery("SELECT 1");
            }
        }

        private IDatabaseConnection GetConnection() {
            var connectionString = "Server=localhost;Database=datafaceintegrationtests;Username=postgresintegrationtests;Password=postgresintegrationtests;";
            if (connectionString != "Server=localhost;Database=datafaceintegrationtests;Username=postgresintegrationtests;Password=postgresintegrationtests;") {
                throw new Exception("Do not change the integration test connection string");
            }
            return new PostgreSqlDatabaseConnection(connectionString);
        }

        private void InitializeDatabase(IDatabaseConnection connection) {
            using (var transaction = connection.BeginTransaction()) {
                foreach (var batch in ReadSqlFileIntoBatches("PostgreSqlIntegrationTests\\CreateTestDatabase.sql")) {
                    if (batch.Trim() != "") {
                        transaction.ExecuteAdHocQuery(batch, new CommandOptions { CommandTimeout = 30 });
                    }
                }

                transaction.Commit();
            }
        }

        private IEnumerable<string> ReadSqlFileIntoBatches(string filename) {
            filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\" + filename;
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
