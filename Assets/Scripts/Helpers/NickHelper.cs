using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems
{
    public static class NickHelper
    {
        public static bool IsRightNick(string nick)
        {
            return nick != null && nick.Length > 3 && !nick.Contains(" ");
        }
    }
}
