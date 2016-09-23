using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFace.Core {
    public interface ICommand : IDisposable {
        List<ResultSet> ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters, CommandOptions commandOptions);
        List<ResultSet> ExecuteAdHocQuery(string adhocQuery, CommandOptions commandOptions);
    }
}
