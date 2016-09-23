using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFace.Core {
    public interface ITransaction : ICommand {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
