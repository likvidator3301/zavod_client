using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems
{
    public static class StringExtension
    {
        public static UnitType ToUnitType(this string tag)
        {
            return (UnitType)Enum.Parse(typeof(UnitType), tag, true);
        }
    }
}
