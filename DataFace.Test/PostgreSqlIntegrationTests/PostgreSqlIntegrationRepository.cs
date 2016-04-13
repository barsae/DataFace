using DataFace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.PostgreSqlIntegrationTests {
    public class PostgreSqlIntegrationRepository : BaseRepository {
        public PostgreSqlIntegrationRepository(IDatabaseConnection connection) : base(connection) {
        }

        public int ToScalar() {
            return ExecuteStoredProcedure(new object[] {}).ToScalar<int>();
        }

        public object ToSingleOrDefault() {
            return ExecuteStoredProcedure(new object[] {}).ToSingleOrDefaultRow<object>();
        }

        public MultipleResultSetModel ToMultipleResultSetModel() {
            return ExecuteStoredProcedure(new object[] {}).ToMultipleResultSetModel<MultipleResultSetModel>();
        }

        public int SprocWithParameter(int parameter) {
            return ExecuteStoredProcedure(new object[] { parameter }).ToScalar<int>();
        }

        public void SprocWithSideEffect() {
            ExecuteStoredProcedure(new object[] {});
        }

        public int GetCountOfSideEffects() {
            return ExecuteStoredProcedure(new object[] {}).ToScalar<int>();
        }

        [Schema("testschema")]
        public int SprocWithSchema() {
            return ExecuteStoredProcedure(new object[] {}).ToScalar<int>();
        }
    }
}
