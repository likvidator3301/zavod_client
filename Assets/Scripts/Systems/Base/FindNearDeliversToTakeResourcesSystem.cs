using System.Linq;
using Systems.BaseHelpers;
using Components;
using Components.Base;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Base
{
    public class FindNearDeliversToTakeResourcesSystem: IEcsRunSystem
    {
        private EcsFilter<MovementComponent, ResourceDeliverComponent> delivers;
        private EcsFilter<BaseComponent> baseComponents;
        
        
        public void Run() => FindNearDeliversToTakeResources();

        private void FindNearDeliversToTakeResources()
        {
            var deliversEntities = delivers.Entities
                .Take(delivers.GetEntitiesCount());
            var baseEntities = baseComponents.Entities
                .Take(baseComponents.GetEntitiesCount());

            foreach (var deliverEntity in deliversEntities)
            {
                var deliverComponent = deliverEntity.Get<ResourceDeliverComponent>();
                var deliverPosition = deliverEntity.Get<MovementComponent>().CurrentPosition;

                foreach (var baseEntity in baseEntities)
                {
                    var basePosition = baseEntity.Get<BaseComponent>().Position;

                    if (BaseTakeResourcesHelpers.CanTakeResourcesFromDeliver(
                        deliverComponent,
                        deliverPosition,
                        basePosition))
                    {
                        BaseTakeResourcesHelpers.CreateTakeResourcesFromDeliverEvent(deliverEntity, baseEntity);
                    }
                }
            }
        }
    }
}