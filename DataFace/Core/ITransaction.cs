using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFace.Core {
    public interface ITransaction : IDisposable {
        List<ResultSet> ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters);
        List<ResultSet> ExecuteAdHocQuery(string adhocQuery);
        void Commit();
        void Rollback();
    }
}
