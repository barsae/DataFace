using DataFace.Core;
using DataFace.SqlServer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.SqlServerIntegrationTests {
    [TestFixture]
    public class SqlServerTests {
        [TestCase]
        public void SqlServer_ToScalar_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new SqlServerIntegrationRepository(GetConnection());
            Assert.AreEqual(142, repo.ToScalar());
        }

        [TestCase]
        public void SqlServer_ToScalarNull_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new SqlServerIntegrationRepository(GetConnection());
            Assert.IsNull(repo.ToScalarNull());
        }

        [TestCase]
        public void SqlServer_ToFirstOrDefault_WithValue_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new SqlServerIntegrationRepository(GetConnection());
            Assert.AreEqual(1, repo.ToFirstOrDefaultScalar1());
        }

        [TestCase]
        public void SqlServer_ToFirstOrDefault_WithoutValue_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new SqlServerIntegrationRepository(GetConnection());
            Assert.IsNull(repo.ToFirstOrDefaultScalar2());
        }

        [TestCase]
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

        [TestCase]
        public void SqlServer_EmptyMultipleResultSets_Work() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new SqlServerIntegrationRepository(GetConnection());
            var model = repo.EmptyMultipleResultSetModel();

            Assert.IsNotNull(model);
        }

        [TestCase]
        public void SqlServer_SprocWithParameter_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new SqlServerIntegrationRepository(GetConnection());
            Assert.AreEqual(143, repo.SprocWithParameter(143));
        }

        [TestCase]
        public void SqlServer_CommitTransaction_HasSideEffect() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new SqlServerIntegrationRepository(GetConnection());
            using (var transaction = repo.WithTransaction()) {
                repo.SprocWithSideEffect();
                transaction.Commit();
            }

            Assert.AreEqual(1, repo.GetCountOfSideEffects());
        }

        [TestCase]
        public void SqlServer_RollbackTransaction_DoesntHaveSideEffect() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new SqlServerIntegrationRepository(GetConnection());
            using (var transaction = repo.WithTransaction()) {
                repo.SprocWithSideEffect();
                transaction.Rollback();
            }

            Assert.AreEqual(0, repo.GetCountOfSideEffects());
        }

        [TestCase]
        public void SqlServer_SprocWithSchema_Works() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            var repo = new SqlServerIntegrationRepository(GetConnection());
            Assert.AreEqual(123, repo.SprocWithSchema());
        }

        [TestCase]
        public void SqlServer_ManyManyTransactions_DoesntLeak() {
            IDatabaseConnection connection = GetConnection();
            InitializeDatabase(connection);

            for (int ii = 0; ii < 1000; ii++) {
                var repo = new SqlServerIntegrationRepository(GetConnection());
                repo.SprocWithSchema();
            }
        }

        [TestCase]
        public void SqlServer_DDLStatement_DoesntCrash() {
            var repo = new SqlServerIntegrationRepository(GetConnection());
            repo.ExecuteAdHocQuery("CREATE DATABASE datafacetestddl");
            repo.ExecuteAdHocQuery("DROP DATABASE datafacetestddl");
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
