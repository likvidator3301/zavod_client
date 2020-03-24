using System;
using Components;
using Components.Resource;
using Leopotam.Ecs;

namespace Systems
{
    public static class TakeResourceHelpers
    {
        public static void CreateTakeEvent(EcsEntity unitEntity, EcsEntity resourceEntity)
        {
            var resourceComponent = resourceEntity.Get<ResourceComponent>();

            var takeResourceEvent = unitEntity.Set<ResourceTakeEvent>();
            takeResourceEvent.Count = resourceComponent.ResourceCount;
            takeResourceEvent.Tag = resourceComponent.Tag;
        }

        public static bool CanBeTaken(EcsEntity unitEntity, EcsEntity resourceEntity)
        {   
            var deliverComponent = unitEntity.Get<ResourceDeliverComponent>();
            var resourceComponent = resourceEntity.Get<ResourceComponent>();
            
            return deliverComponent.MoneyCount + deliverComponent.SemkiCount + resourceComponent.ResourceCount <=
                   deliverComponent.MaxResourceCount;
        }
    }
}