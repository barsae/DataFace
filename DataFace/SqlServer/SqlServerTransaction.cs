using DataFace.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataFace.SqlServer {
    public class SqlServerTransaction : ITransaction {
        private SqlServerDatabaseConnection databaseConnection;

        public SqlServerTransaction(SqlServerDatabaseConnection databaseConnection) {
            this.databaseConnection = databaseConnection;
        }

        public List<ResultSet> ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters) {
            using (var sqlConnection = new SqlConnection(databaseConnection.ConnectionString)) {
                sqlConnection.Open();

                using (var command = new SqlCommand()) {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedureName;
                    command.Connection = sqlConnection;

                    foreach (var parameter in parameters) {
                        command.Parameters.Add(new SqlParameter(parameter.Key, parameters.Values));
                    }

                    using (var reader = command.ExecuteReader()) {
                        return ConvertReaderToMultipleResultSet(reader);
                    }
                }
            }
        }

        public List<ResultSet> ExecuteAdHocQuery(string adhocQuery) {
            using (var sqlConnection = new SqlConnection(databaseConnection.ConnectionString)) {
                sqlConnection.Open();

                using (var command = new SqlCommand()) {
                    command.CommandType = CommandType.Text;
                    command.CommandText = adhocQuery;
                    command.Connection = sqlConnection;

                    using (var reader = command.ExecuteReader()) {
                        return ConvertReaderToMultipleResultSet(reader);
                    }
                }
            }
        }

        public void Dispose() {
        }

        private List<ResultSet> ConvertReaderToMultipleResultSet(SqlDataReader reader) {
            var resultSets = new List<ResultSet>();

            do {
                var resultSet = new ResultSet();
                resultSet.Columns = GetColumns(reader);
                resultSet.Rows = GetRows(reader);
                resultSets.Add(resultSet);
            } while (reader.NextResult());

            return resultSets;
        }

        private List<Column> GetColumns(SqlDataReader reader) {
            var columns = new List<Column>();
            for (int ii = 0; ii < reader.FieldCount; ii++) {
                columns.Add(new Column(reader.GetName(ii)));
            }
            return columns;
        }

        private List<Row> GetRows(SqlDataReader reader) {
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
