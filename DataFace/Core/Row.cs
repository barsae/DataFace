using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFace.Core {
    public class Row {
        public List<object> Values { get; set; }

        public Row(List<object> values) {
            this.Values = values;
        }
    }
}
