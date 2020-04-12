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
            foreach (var takeEntity in takeEntities)
            {
                var deliverComponent = takeEntity.Get<ResourceDeliverComponent>();
                var takeEvent = takeEntity.Get<ResourceTakeEvent>();
                deliverComponent.Resources.Add(takeEvent.Resource);
                
                takeEntity.Unset<ResourceTakeEvent>();
            }
        }
    }
}