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
        private CommandOptions defaultCommandOptions;

        public BaseRepository(IDatabaseConnection connection) {
            this.connection = connection;
            defaultCommandOptions = new CommandOptions { CommandTimeout = 30 };
        }

        public MultipleResultSetConverter ExecuteStoredProcedure([CallerMemberName]string sprocName = "") {
            return ExecuteStoredProcedure(sprocName, new Dictionary<string, object>(), defaultCommandOptions);
        }

        public MultipleResultSetConverter ExecuteStoredProcedure(CommandOptions commandOptions, [CallerMemberName]string sprocName = "") {
            return ExecuteStoredProcedure(sprocName, new Dictionary<string, object>(), commandOptions);
        }

        public MultipleResultSetConverter ExecuteStoredProcedure<InputModel>(InputModel inputModel, [CallerMemberName]string sprocName = "") {
            return ExecuteStoredProcedure(sprocName, GetParameters(inputModel), defaultCommandOptions);
        }

        public MultipleResultSetConverter ExecuteStoredProcedure<InputModel>(InputModel inputModel, CommandOptions commandOptions, [CallerMemberName]string sprocName = "") {
            return ExecuteStoredProcedure(sprocName, GetParameters(inputModel), commandOptions);
        }

        public MultipleResultSetConverter ExecuteAdHocQuery(string adhocQuery) {
            return ExecuteAdHocQuery(adhocQuery, defaultCommandOptions);
        }

        public MultipleResultSetConverter ExecuteAdHocQuery(string adhocQuery, CommandOptions commandOptions) {
            return WithTransaction((transaction) => {
                return new MultipleResultSetConverter(transaction.ExecuteAdHocQuery(adhocQuery, commandOptions));
            });
        }

        public TransactionContext WithTransaction() {
            transactionContext = new TransactionContext(this, connection.BeginTransaction());
            return transactionContext;
        }

        private MultipleResultSetConverter ExecuteStoredProcedure(string sprocName, Dictionary<string, object> parameters, CommandOptions commandOptions) {
            var schemaPrefix = GetSchemaPrefix(sprocName);
            return WithTransaction((transaction) => {
                return new MultipleResultSetConverter(transaction.ExecuteStoredProcedure(schemaPrefix + sprocName, parameters, commandOptions));
            });
        }

        private ReturnType WithTransaction<ReturnType>(Func<ITransaction, ReturnType> func) {
            if (transactionContext != null) {
                return func(transactionContext.Transaction);
            }
            else {
                using (var transaction = connection.BeginTransaction()) {
                    var result = func(transaction);
                    transaction.Commit();
                    return result;
                }
            }
        }

        private Dictionary<string, object> GetParameters<InputModel>(InputModel inputModel) {
            return inputModel.GetType()
                             .GetProperties()
                             .ToDictionary(a => a.Name, b => b.GetValue(inputModel));
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
