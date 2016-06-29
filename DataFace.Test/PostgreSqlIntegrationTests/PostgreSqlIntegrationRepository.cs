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
            return ExecuteStoredProcedure().ToScalar<int>();
        }

        public object ToSingleOrDefault() {
            return ExecuteStoredProcedure().ToSingleOrDefaultRow<object>();
        }

        public MultipleResultSetModel ToMultipleResultSetModel() {
            return ExecuteStoredProcedure().ToMultipleResultSetModel<MultipleResultSetModel>();
        }

        public int SprocWithParameter(int parameter) {
            var input = new SprocInputModel() {
                Parameter = parameter
            };
            return ExecuteStoredProcedure(input).ToScalar<int>();
        }

        public void SprocWithSideEffect() {
            ExecuteStoredProcedure();
        }

        public int GetCountOfSideEffects() {
            return ExecuteStoredProcedure().ToScalar<int>();
        }

        [Schema("testschema")]
        public int SprocWithSchema() {
            return ExecuteStoredProcedure().ToScalar<int>();
        }
    }
}
