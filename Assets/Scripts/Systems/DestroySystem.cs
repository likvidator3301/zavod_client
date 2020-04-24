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
        private readonly EcsFilter<DestroyEvent, ResourceDeliverComponent> delivers;
        private readonly EcsWorld world;
        
        public void Run()
        {
            DropResources();
            DestroyEntities();
        }

        private void DestroyEntities()
        {
            var destroyEntities = destroyEvents.Entities
                .Take(destroyEvents.GetEntitiesCount());
            foreach (var destroyEntity in destroyEntities)
            {
                var objectToDestroy = destroyEntity.Get<DestroyEvent>().Object;
                if (objectToDestroy != null)
                    Object.Destroy(objectToDestroy);
                
                destroyEntity.Destroy();
            }
        }

        private void DropResources()
        {
            var deliversEntities = delivers.Entities
                .Take(delivers.GetEntitiesCount())
                .Where(e => e.Get<MovementComponent>().IsObjectAlive);
            foreach (var deliverEntity in deliversEntities)
                DropResourceHelpers.CreateDropResourcesEvent(deliverEntity, world);
        }
    }
}