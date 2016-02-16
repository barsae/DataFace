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
            return ExecuteStoredProcedure(new object[] {}).ToScalar<Int64>();
        }

        public List<Int64> ToScalars() {
            return ExecuteStoredProcedure(new object[] {}).ToScalars<Int64>();
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

        public int InsertRecord() {
            return ExecuteStoredProcedure(new object[] {}).ToScalar<int>();
        }

        [Schema("testschema")]
        public int SprocWithSchema() {
            return ExecuteStoredProcedure(new object[] {}).ToScalar<int>();
        }
    }
}
