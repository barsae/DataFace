using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFace.Core {
    public class DataFaceException : Exception {
        public DataFaceException(string message) : base(message) {
        }
    }
}
