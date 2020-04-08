using System.Linq;
using Components;
using Components.Health;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class DestroySystem: IEcsRunSystem
    {
        private readonly EcsFilter<DestroyEvent, UnitComponent> destroyEvents;
        
        public void Run() => DestroyEntities();

        private void DestroyEntities()
        {
            var destroyEntities = destroyEvents.Entities.Where(e => e.IsNotNullAndAlive());
            foreach (var destroyEntity in destroyEntities)
            {
                Object.Destroy(destroyEntity.Get<DestroyEvent>().Object);
                destroyEntity.Destroy();
            }
        }
    }
}