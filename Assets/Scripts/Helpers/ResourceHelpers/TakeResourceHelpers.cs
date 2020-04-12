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
            takeResourceEvent.Resource = resourceComponent;
        }

        public static bool CanBeTaken(EcsEntity unitEntity)
        {   
            var deliverComponent = unitEntity.Get<ResourceDeliverComponent>();

            return deliverComponent.MaxResourcesTakenCount > deliverComponent.Resources.Count;
        }
    }
}