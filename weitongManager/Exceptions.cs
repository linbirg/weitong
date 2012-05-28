using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace weitongManager
{
    class ZeroStorageException :ApplicationException
    {
        public ZeroStorageException(string str = "库存为零"):base(str)
        {
            
        }
    }

    class ZeroCartException : ApplicationException
    {
        public ZeroCartException(string str = "购物车中酒的数量为零") : base(str) { }
    }

    class InvalidWineCodeException : ApplicationException
    {
        public InvalidWineCodeException(string msg = "错误的酒编码") : base(msg) { }
    }
}
