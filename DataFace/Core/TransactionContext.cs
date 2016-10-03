using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Core {
    public class TransactionContext : IDisposable {
        public ITransaction Transaction { get; private set; }

        private IBaseRepository repository;

        public TransactionContext(IBaseRepository repository, ITransaction transaction) {
            this.Transaction = transaction;
            this.repository = repository;
        }

        public void Commit() {
            Transaction.Commit();
        }

        public void Rollback() {
            Transaction.Rollback();
        }

        public void Dispose() {
            Transaction.Dispose();
            repository.transactionContext = null;
        }
    }
}
