using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataFace {
    public class BaseRepository {
        private IDatabaseConnection connection;

        public BaseRepository(IDatabaseConnection connection) {
            this.connection = connection;
        }

        public MultipleResultSetConverter ExecuteStoredProcedure(object[] rawParameters, [CallerMemberName]string sprocName = "") {
            var parameters = GetParameters(sprocName, rawParameters);
            using (var transaction = connection.BeginTransaction()) {
                return new MultipleResultSetConverter(transaction.ExecuteStoredProcedure(sprocName, parameters));
            }
        }

        private Dictionary<string, object> GetParameters(string sprocName, object[] rawParameters) {
            return GetType().GetMethod(sprocName)
                            .GetParameters()
                            .Zip(rawParameters, (param, value) => new KeyValuePair<string, object>(param.Name, value))
                            .ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);
        }
    }
}
