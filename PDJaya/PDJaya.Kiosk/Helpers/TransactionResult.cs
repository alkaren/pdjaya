using PDJaya.Kiosk.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDJaya.Kiosk
{
    public class TransactionResult
    {
        public bool IsSucceed { get; set; }
        public object Parameters { get; set; }
        public string Message { get; set; }
        public ResultTypes Output { get; set; }
    }
}
