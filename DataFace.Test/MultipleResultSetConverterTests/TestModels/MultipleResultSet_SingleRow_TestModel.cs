using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.MultipleResultSetConverterTests.TestModels {
    public class MultipleResultSet_SingleRow_TestModel {
        [ResultSet(0, ResultSetType.SingleRow)]
        public MultipleResultSet_SingleRow_TestModel_Row SingleRow { get; set; }
    }

    public class MultipleResultSet_SingleRow_TestModel_Row {
        public int Value { get; set; }
    }
}
