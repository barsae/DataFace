using DataFace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.MultipleResultSetConverterTests.TestModels {
    public class MultipleResultSet_SingleOrDefaultRow_TestModel {
        [ResultSet(0, ResultSetType.SingleOrDefaultRow)]
        public MultipleResultSet_SingleOrDefaultRow_TestModel_Row SingleOrDefaultRow { get; set; }
    }

    public class MultipleResultSet_SingleOrDefaultRow_TestModel_Row {
        public int Value { get; set; }
    }
}
