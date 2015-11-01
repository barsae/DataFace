using DataFace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.MultipleResultSetConverterTests.TestModels {
    public class MultipleResultSet_Rows_TestModel {
        [ResultSet(0, ResultSetType.Rows)]
        public List<MultipleResultSet_Rows_TestModel_Row> Rows { get; set; }
    }

    public class MultipleResultSet_Rows_TestModel_Row {
        public int Value { get; set; }
    }
}
