using System.Runtime.CompilerServices;

namespace DataFace.Core {
    public interface IBaseRepository {
        object commandContext { get; set; }
        TransactionContext transactionContext { get; set; }

        MultipleResultSetConverter ExecuteAdHocQuery(string adhocQuery);
        MultipleResultSetConverter ExecuteAdHocQuery(string adhocQuery, CommandOptions commandOptions);
        MultipleResultSetConverter ExecuteStoredProcedure([CallerMemberName] string sprocName = "");
        MultipleResultSetConverter ExecuteStoredProcedure(CommandOptions commandOptions, [CallerMemberName] string sprocName = "");
        MultipleResultSetConverter ExecuteStoredProcedure<InputModel>(InputModel inputModel, [CallerMemberName] string sprocName = "");
        MultipleResultSetConverter ExecuteStoredProcedure<InputModel>(InputModel inputModel, CommandOptions commandOptions, [CallerMemberName] string sprocName = "");
        TransactionContext WithTransaction();
    }
}