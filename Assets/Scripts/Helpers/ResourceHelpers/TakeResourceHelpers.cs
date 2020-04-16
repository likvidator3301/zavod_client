using Components;
using Components.Resource;
using Leopotam.Ecs;

namespace Systems
{
    public static class TakeResourceHelpers
    {
        public static void CreateTakeEvent(EcsEntity deliverEntity, EcsEntity resourceEntity)
        {
            deliverEntity.Set<ResourceTakeEvent>().ResourceEntity = resourceEntity;
        }

        public static bool CanBeTaken(EcsEntity unitEntity)
        {   
            var deliverComponent = unitEntity.Get<ResourceDeliverComponent>();

            return deliverComponent.MaxResourcesTakenCount > deliverComponent.Resources.Count;
        }
    }
}