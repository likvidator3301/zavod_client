using System;
using Components;
using Extensions;
using Leopotam.Ecs;
using Models;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public static class UnitPrefabsHolderExtensions
    {
        private const float destinationAccuracy = 0.5f;
        

        public static void AddNewDeliverEntityOnPosition(
            this GameObject prefab, EcsWorld world, Vector3 position, Guid guid, UnitTag tag = UnitTag.Runner)
        {
            var newUnitObject = prefab.InstantiateNewObject(position, Quaternion.identity);
            var newEntity = world.NewEntityWith<UnitComponent, NavMeshComponent, HealthBarComponent, ResourceDeliverComponent>(
                out var unitComponent,
                out var navMeshAgentComponent,
                out var healthBarComponent,
                out var deliverComponent);
            
            //healthBarComponent = ...
            //unitAnimationComponent.Animator = newUnitObject.GetComponent<Animator>();
            navMeshAgentComponent.Agent = newUnitObject.GetComponent<NavMeshAgent>();
            navMeshAgentComponent.Agent.stoppingDistance = destinationAccuracy;
            unitComponent.SetFields(
                newUnitObject, tag, guid);
            newEntity.AddDeliverComponents(position, newUnitObject);
            deliverComponent.MaxResourceCount = 150;
        }
    }
}
