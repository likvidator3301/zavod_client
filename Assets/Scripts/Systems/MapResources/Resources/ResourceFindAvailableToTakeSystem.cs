using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Leopotam.Ecs;
using ServerCommunication;
using UnityEngine;

namespace Systems.Zavod
{
    public class ResourceFindAvailableToTakeSystem: IEcsRunSystem
    {
        private readonly EcsFilter<MovementComponent, ResourceDeliverComponent, MyUnitComponent> delivers;
        private readonly EcsFilter<ResourceComponent>.Exclude<EntityDestroyedSoonComponent> resources;
        private const float minTakingDistance = 1.5f;
        
        public void Run() => FindAvailableToTakeResources();

        private void FindAvailableToTakeResources()
        {
            var takenResourcesGuids = new List<Guid>();
            var deliversEntities = delivers.Entities
                .Take(delivers.GetEntitiesCount());
            var resourcesEntities = resources.Entities
                .Take(resources.GetEntitiesCount());
            
            foreach (var deliverEntity in deliversEntities)
            {
                var deliverPosition = deliverEntity.Get<MovementComponent>().CurrentPosition;
                foreach (var resourceEntity in resourcesEntities)
                {
                    var resourceGuid = resourceEntity.Get<ResourceComponent>().Guid;
                    if (takenResourcesGuids.Contains(resourceGuid))
                        continue;
                    
                    var resourcePosition = resourceEntity.Get<ResourceComponent>().Position;
                    if (Vector3.Distance(resourcePosition, deliverPosition) <= minTakingDistance)
                    {
                        if (TakeResourceHelpers.CanBeTaken(deliverEntity))
                        {
                            TakeResourceHelpers.CreateTakeEvent(deliverEntity, resourceEntity);
                            ResourceDestroyHelpers.CreateDestroyEvent(resourceEntity);
                            takenResourcesGuids.Add(resourceGuid);
                        }
                        break;
                    }
                }
            }
        }
    }
}