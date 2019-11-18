using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public class UnitsQueueComponent
    {
        public Queue<UnitCreateEvent> units = new Queue<UnitCreateEvent>();
    }
}
