using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Communication
{
    public class ServerEventsQueue
    {
        public Queue<UnitPositionUpdate> UnitPositionsEvents = new Queue<UnitPositionUpdate>();
    }
}
