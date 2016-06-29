using DataFace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.MySqlIntegrationTests {
    public class MySqlIntegrationRepository : BaseRepository {
        public MySqlIntegrationRepository(IDatabaseConnection connection) : base(connection) {
        }

        public Int64 ToScalar() {
            return ExecuteStoredProcedure().ToScalar<Int64>();
        }

        public List<Int64> ToScalars() {
            return ExecuteStoredProcedure().ToScalars<Int64>();
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

        public int InsertRecord() {
            return ExecuteStoredProcedure().ToScalar<int>();
        }

        [Schema("testschema")]
        public int SprocWithSchema() {
            return ExecuteStoredProcedure().ToScalar<int>();
        }
    }
}
