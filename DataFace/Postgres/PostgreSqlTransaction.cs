using DataFace.Core;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataFace.PostgreSql {
    public class PostgreSqlTransaction : ITransaction {
        private NpgsqlConnection connection;
        private NpgsqlTransaction transaction;

        public PostgreSqlTransaction(PostgreSqlDatabaseConnection databaseConnection) {
            this.connection = new NpgsqlConnection(databaseConnection.ConnectionString);
            this.connection.Open();
            this.transaction = this.connection.BeginTransaction();
        }

        public List<ResultSet> ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters) {
            using (var command = new NpgsqlCommand()) {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;
                command.Connection = connection;
                command.Transaction = transaction;

                foreach (var parameter in parameters) {
                    command.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));
                }

                using (var reader = command.ExecuteReader()) {
                    return ConvertReaderToMultipleResultSet(reader, procedureName.ToLower());
                }
            }
        }

        public List<ResultSet> ExecuteAdHocQuery(string adhocQuery) {
            using (var command = new NpgsqlCommand()) {
                command.CommandType = CommandType.Text;
                command.CommandText = adhocQuery;
                command.Connection = connection;
                command.Transaction = transaction;

                using (var reader = command.ExecuteReader()) {
                    return ConvertReaderToMultipleResultSet(reader, null);
                }
            }
        }

        public void Commit() {
            transaction.Commit();
        }

        public void Rollback() {
            transaction.Rollback();
        }

        public void Dispose() {
            transaction.Dispose();
        }

        private List<ResultSet> ConvertReaderToMultipleResultSet(NpgsqlDataReader reader, string procedureName) {
            var resultSets = new List<ResultSet>();

            do {
                var resultSet = new ResultSet();
                resultSet.Columns = GetColumns(reader, procedureName);
                resultSet.Rows = GetRows(reader, procedureName);
                resultSets.Add(resultSet);
            } while (reader.NextResult());

            return resultSets;
        }

        private List<Column> GetColumns(NpgsqlDataReader reader, string procedureName) {
            var columns = new List<Column>();
            for (int ii = 0; ii < reader.FieldCount; ii++) {
                var name = reader.GetName(ii);
                if (name != procedureName) {
                    columns.Add(new Column(name));
                }
            }
            return columns;
        }

        private List<Row> GetRows(NpgsqlDataReader reader, string procedureName) {
            var rows = new List<Row>();
            while (reader.Read()) {
                var row = new List<object>();
                for (int ii = 0; ii < reader.FieldCount; ii++) {
                    var name = reader.GetName(ii);
                    if (name != procedureName) {
                        row.Add(reader.GetValue(ii));
                    }
                }
                rows.Add(new Row(row));
            }
            return rows;
        }
    }
}
