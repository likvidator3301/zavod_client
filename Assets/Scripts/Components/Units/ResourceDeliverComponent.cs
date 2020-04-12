using System.Collections.Generic;

namespace Components
{
    public class ResourceDeliverComponent
    {
        public List<ResourceComponent> Resources = new List<ResourceComponent>();
        public int MaxResourcesTakenCount { get; set; }
    }
}