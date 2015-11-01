﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFace.Core {
    public interface IResultSet {
        List<Column> GetColumns();
        List<IRow> GetRows();
    }
}