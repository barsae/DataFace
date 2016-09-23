using DataFace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.MySql {
    public class MySqlDatabaseConnection : IDatabaseConnection {
        public string ConnectionString { get; set; }

        public MySqlDatabaseConnection(string connectionString) {
            this.ConnectionString = connectionString;
        }

        public ICommand BeginCommand() {
            return new MySqlTransaction(this);
        }

        public ITransaction BeginTransaction() {
            var transaction = new MySqlTransaction(this);
            transaction.BeginTransaction();
            return transaction;
        }
    }
}
