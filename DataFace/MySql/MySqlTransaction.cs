using DataFace.Core;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using RawConnection = MySql.Data.MySqlClient.MySqlConnection;
using RawTransaction = MySql.Data.MySqlClient.MySqlTransaction;

namespace DataFace.MySql {
    public class MySqlTransaction : ITransaction, IDisposable {
        private RawConnection connection;
        private RawTransaction transaction;

        public int CommandTimeout { get; set; }

        public MySqlTransaction(MySqlDatabaseConnection databaseConnection) {
            this.connection = new RawConnection(databaseConnection.ConnectionString);
        }

        public List<ResultSet> ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters, CommandOptions commandOptions) {
            if (procedureName.Contains('.')) {
                throw new ArgumentException("MySql does not support schemas");
            }

            Open();
            using (var command = new MySqlCommand()) {
                command.CommandTimeout = commandOptions.CommandTimeout;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;
                command.Connection = connection;
                command.Transaction = transaction;

                foreach (var parameter in parameters) {
                    command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
                }

                using (var reader = command.ExecuteReader()) {
                    return ConvertReaderToMultipleResultSet(reader);
                }
            }
        }

        public List<ResultSet> ExecuteAdHocQuery(string adhocQuery, CommandOptions commandOptions) {
            Open();
            using (var command = new MySqlCommand()) {
                command.CommandTimeout = commandOptions.CommandTimeout;
                command.CommandType = CommandType.Text;
                command.CommandText = adhocQuery;
                command.Connection = connection;
                command.Transaction = transaction;

                using (var reader = command.ExecuteReader()) {
                    return ConvertReaderToMultipleResultSet(reader);
                }
            }
        }

        public void BeginTransaction() {
            Open();
            transaction = connection.BeginTransaction();
        }

        public void Commit() {
            transaction.Commit();
        }

        public void Rollback() {
            transaction.Rollback();
        }

        public void Dispose() {
            if (transaction != null) {
                transaction.Dispose();
            }

            connection.Dispose();
        }

        private void Open() {
            if (connection.State == ConnectionState.Closed) {
                connection.Open();
            }
        }

        private List<ResultSet> ConvertReaderToMultipleResultSet(MySqlDataReader reader) {
            var resultSets = new List<ResultSet>();

            do {
                var resultSet = new ResultSet();
                resultSet.Columns = GetColumns(reader);
                resultSet.Rows = GetRows(reader);
                resultSets.Add(resultSet);
            } while (reader.NextResult());

            return resultSets;
        }

        private List<Column> GetColumns(MySqlDataReader reader) {
            var columns = new List<Column>();
            for (int ii = 0; ii < reader.FieldCount; ii++) {
                columns.Add(new Column(reader.GetName(ii)));
            }
            return columns;
        }

        private List<Row> GetRows(MySqlDataReader reader) {
            var rows = new List<Row>();
            while (reader.Read()) {
                var row = new List<object>();
                for (int ii = 0; ii < reader.FieldCount; ii++) {
                    row.Add(reader.GetValue(ii));
                }
                rows.Add(new Row(row));
            }
            return rows;
        }
    }
}
