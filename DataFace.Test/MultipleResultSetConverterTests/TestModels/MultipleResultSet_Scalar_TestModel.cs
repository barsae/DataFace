using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.MultipleResultSetConverterTests.TestModels {
    public class MultipleResultSet_Scalar_TestModel {
        [ResultSet(0, ResultSetType.Scalar)]
        public int Value { get; set; }
    }
}
