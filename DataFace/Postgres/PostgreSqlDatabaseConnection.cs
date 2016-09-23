using DataFace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.PostgreSql {
    public class PostgreSqlDatabaseConnection : IDatabaseConnection {
        public string ConnectionString { get; set; }

        public PostgreSqlDatabaseConnection(string connectionString) {
            this.ConnectionString = connectionString;
        }

        public ICommand BeginCommand() {
            return new PostgreSqlTransaction(this);
        }

        public ITransaction BeginTransaction() {
            var transaction = new PostgreSqlTransaction(this);
            transaction.BeginTransaction();
            return transaction;
        }
    }
}
