using System.Linq;
using Components;
using Components.Health;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class DestroySystem: IEcsRunSystem
    {
        private readonly EcsFilter<DestroyEvent> destroyEvents;
        
        public void Run() => DestroyEntities();

        private void DestroyEntities()
        {
            var destroyEntities = destroyEvents.Entities
                .Take(destroyEvents.GetEntitiesCount());
            foreach (var destroyEntity in destroyEntities)
            {
                Object.Destroy(destroyEntity.Get<DestroyEvent>().Object);
                destroyEntity.Destroy();
            }
        }
    }
}