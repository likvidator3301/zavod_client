using System.Collections.Generic;
using Leopotam.Ecs;

namespace Components.Resource
{
    public class ResourceDropEvent
    {
        [EcsIgnoreNullCheck]
        public List<ResourceComponent> Resources;
    }
}