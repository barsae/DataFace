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

        public ITransaction BeginTransaction() {
            return new MySqlTransaction(this);
        }
    }
}
