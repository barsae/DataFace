using DataFace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Test.MultipleResultSetConverterTests.TestModels {
    public class MultipleResultSet_Scalars_TestModel {
        [ResultSet(0, ResultSetType.Scalars)]
        public List<int> Values { get; set; }
    }
}
