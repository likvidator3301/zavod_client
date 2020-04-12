using Components;
using Components.Base;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.BaseHelpers
{
    public class BaseTakeResourcesHelpers
    {
        private const float minDistanceBetweenBaseAndDeliver = 10;
        
        
        public static void CreateTakeResourcesFromDeliverEvent(EcsEntity deliverEntity, EcsEntity baseEntity)
        {
            baseEntity.Set<TakeResourcesFromDeliverEvent>().Resources =
                deliverEntity.Get<ResourceDeliverComponent>().Resources;
        }
        
        public static bool CanTakeResourcesFromDeliver(
            ResourceDeliverComponent deliverComponent,
            Vector3 deliverPosition,
            Vector3 basePosition)
        {
            return Vector3.Distance(deliverPosition, basePosition) <= minDistanceBetweenBaseAndDeliver
                   && deliverComponent.Resources.Count > 0;
        }
    }
}