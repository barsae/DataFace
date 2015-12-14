using DataFace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFace.Test.PostgreSqlIntegrationTests {
    public class MultipleResultSetModel {
        [ResultSet(0, ResultSetType.Rows)]
        public List<MultipleResultSetModel_ResultSet0> ResultSet0 { get; set; }

        [ResultSet(1, ResultSetType.Rows)]
        public List<MultipleResultSetModel_ResultSet1> ResultSet1 { get; set; }
    }

    public class MultipleResultSetModel_ResultSet0 {
        public int IntValue { get; set; }
        public bool BoolValue { get; set; }
    }

    public class MultipleResultSetModel_ResultSet1 {
        public string StringValue { get; set; }
        public DateTime DateTimeValue { get; set; }
    }
}
