using System.Linq;
using Components;
using Components.Resource;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Zavod
{
    public class ResourceTakeSystem: IEcsRunSystem
    {
        private readonly EcsFilter<ResourceDeliverComponent, ResourceTakeEvent> takeEvents;
        
        public void Run() => TakeResource();

        private void TakeResource()
        {
            var takeEntities = takeEvents.Entities
                .Take(takeEvents.GetEntitiesCount());
            foreach (var takeResourceEntity in takeEntities)
            {
                var deliverComponent = takeResourceEntity.Get<ResourceDeliverComponent>();
                var takeEvent = takeResourceEntity.Get<ResourceTakeEvent>();
                deliverComponent.Resources.Add(takeEvent.ResourceEntity.Get<ResourceComponent>());
                ResourceDestroyHelpers.CreateDestroyEvent(takeEvent.ResourceEntity);
                
                takeResourceEntity.Unset<ResourceTakeEvent>();
            }
        }
    }
}