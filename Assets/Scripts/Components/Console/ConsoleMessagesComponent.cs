using Leopotam.Ecs;
using System.Collections.Generic;

namespace Components
{
    public class ConsoleMessagesComponent
    {
        [EcsIgnoreNullCheck]
        public List<Message> Messages;
    }
}
