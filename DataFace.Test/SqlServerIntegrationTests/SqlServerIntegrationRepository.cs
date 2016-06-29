using DataFace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.SqlServerIntegrationTests {
    public class SqlServerIntegrationRepository : BaseRepository {
        public SqlServerIntegrationRepository(IDatabaseConnection connection) : base(connection) {
        }

        public int ToScalar() {
            return ExecuteStoredProcedure().ToScalar<int>();
        }

        public MultipleResultSetModel ToMultipleResultSetModel() {
            return ExecuteStoredProcedure().ToMultipleResultSetModel<MultipleResultSetModel>();
        }

        public MultipleResultSetModel EmptyMultipleResultSetModel() {
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
