using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public class AttackInfo
    {
        public Guid AttackUnit;
        public Guid DamagedUnit;
        public int Hp;

        public AttackInfo(Guid attackUnit, Guid damagedUnit, int hp)
        {
            AttackUnit = attackUnit;
            DamagedUnit = damagedUnit;
            Hp = hp;
        }
    }
}
