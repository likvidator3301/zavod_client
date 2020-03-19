using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Communication
{
    public class QueueSendEventsComponent
    {
        [EcsIgnoreNullCheck]
        public ServerEventsQueue Queue = new ServerEventsQueue();
    }
}
