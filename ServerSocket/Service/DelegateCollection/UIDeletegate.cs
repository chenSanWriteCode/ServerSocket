using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Service.DelegateCollection
{
    public  class UIDeletegate
    {
        public delegate void ChangeControlWithStr(string msg);

        public delegate void ChangeControlWithList<T>(List<T> list);

        public delegate void ChangeControl();
    }
}
