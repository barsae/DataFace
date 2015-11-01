using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFace.Core {
    public interface IRow {
        List<object> GetValues();
    }
}
