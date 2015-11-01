using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFace.Core {
    public interface ITransaction : IDisposable {
        List<IResultSet> ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters);
        List<IResultSet> ExecuteAdHocQuery(string adhocQuery);
    }
}
