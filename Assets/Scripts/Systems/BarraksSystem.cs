using System;
using Leopotam.Ecs;
using Components;

namespace Systems
{
    public class BarraksSystem : IEcsRunSystem
    {
        private readonly GameDefinitions gameDefinitions = null;
        private readonly EcsWorld world = null;
        private readonly EcsFilter<UnitCreateEvent> unitCreateEvents = null;

        public void Run()
        {
            for (var i = 0; i < unitCreateEvents.GetEntitiesCount(); i++)
            {
                
            }
        }
    }
}
