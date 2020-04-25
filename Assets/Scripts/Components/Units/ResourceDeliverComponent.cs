using System.Collections.Generic;
using Leopotam.Ecs;

namespace Components
{
    public class ResourceDeliverComponent
    {
        [EcsIgnoreNullCheck]
        public List<ResourceComponent> Resources = new List<ResourceComponent>();

        public int MaxResourcesTakenCount { get; set; } = 3;
    }
}