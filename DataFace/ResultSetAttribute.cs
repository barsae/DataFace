using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace {
    public class ResultSetAttribute : Attribute {
        public int Index { get; set; }
        public ResultSetType Type { get; set; }

        public ResultSetAttribute(int index, ResultSetType type) {
            this.Index = index;
            this.Type = type;
        }
    }
}
