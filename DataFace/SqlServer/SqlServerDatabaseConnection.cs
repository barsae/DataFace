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

        public ITransaction BeginTransaction() {
            return new SqlServerTransaction(this);
        }
    }
}
