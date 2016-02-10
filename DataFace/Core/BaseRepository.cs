using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Core {
    public class BaseRepository {
        private IDatabaseConnection connection;
        private TransactionContext transactionContext;

        public BaseRepository(IDatabaseConnection connection) {
            this.connection = connection;
        }

        public MultipleResultSetConverter ExecuteStoredProcedure(object[] rawParameters, [CallerMemberName]string sprocName = "") {
            var parameters = GetParameters(sprocName, rawParameters);
            var schemaPrefix = GetSchemaPrefix(sprocName);
            return WithTransaction((transaction) => {
                return new MultipleResultSetConverter(transaction.ExecuteStoredProcedure(schemaPrefix + sprocName, parameters));
            });
        }

        public MultipleResultSetConverter ExecuteAdHocQuery(string adhocQuery) {
            return WithTransaction((transaction) => {
                return new MultipleResultSetConverter(transaction.ExecuteAdHocQuery(adhocQuery));
            });
        }

        public TransactionContext WithTransaction() {
            transactionContext = new TransactionContext(this, connection.BeginTransaction());
            return transactionContext;
        }

        private ReturnType WithTransaction<ReturnType>(Func<ITransaction, ReturnType> func) {
            if (transactionContext != null) {
                return func(transactionContext.Transaction);
            } else {
                using (var transaction = connection.BeginTransaction()) {
                    var result = func(transaction);
                    transaction.Commit();
                    return result;
                }
            }
        }

        private Dictionary<string, object> GetParameters(string sprocName, object[] rawParameters) {
            return GetType().GetMethod(sprocName)
                            .GetParameters()
                            .Zip(rawParameters, (param, value) => new KeyValuePair<string, object>(param.Name, value))
                            .ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);
        }

        private string GetSchemaPrefix(string sprocName) {
            var schemaAttribute = (SchemaAttribute)GetType().GetMethod(sprocName)
                                                            .GetCustomAttributes(typeof(SchemaAttribute), false)
                                                            .FirstOrDefault();
            if (schemaAttribute != null) {
                return schemaAttribute.Schema + ".";
            }
            return "";
        }


        public class TransactionContext : IDisposable {
            public ITransaction Transaction { get; private set; }

            private BaseRepository repository;

            public TransactionContext(BaseRepository repository, ITransaction transaction) {
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
}
