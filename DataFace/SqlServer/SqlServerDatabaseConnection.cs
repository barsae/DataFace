using DataFace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.SqlServer {
    public class SqlServerDatabaseConnection : IDatabaseConnection {
        public string ConnectionString { get; set; }

        public SqlServerDatabaseConnection(string connectionString) {
            this.ConnectionString = connectionString;
        }

        public ICommand BeginCommand() {
            return new SqlServerTransaction(this);
        }

        public ITransaction BeginTransaction() {
            var transaction = new SqlServerTransaction(this);
            transaction.BeginTransaction();
            return transaction;
        }
    }
}
