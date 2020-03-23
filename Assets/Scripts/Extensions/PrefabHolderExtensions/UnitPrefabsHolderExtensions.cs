using Components;
using Extensions;
using Leopotam.Ecs;
using Models;
using UnityEngine;
using UnityEngine.AI;

namespace Systems
{
    public static class UnitPrefabsHolderExtensions
    {
        private const float destinationAccuracy = 0.5f;
        
        //TODO: find healthBar in unitObject
        public static void AddNewUnitEntityFromUnitDbo(
            this GameObject prefab, EcsWorld ecsWorld, ServerUnitDto unitDto)
        {
            var newUnitObject = prefab.InstantiateNewObject(unitDto.Position, Quaternion.identity);
            var newEntity = ecsWorld.NewEntityWith<UnitComponent, UnitAnimationComponent, NavMeshComponent, HealthBarComponent>(
                out var unitComponent,
                out var unitAnimationComponent,
                out var navMeshAgentComponent,
                out var healthBarComponent);
            
            //healthBarComponent = ...
            unitAnimationComponent.Animator = newUnitObject.GetComponent<Animator>();
            navMeshAgentComponent.Agent = newUnitObject.GetComponent<NavMeshAgent>();
            navMeshAgentComponent.Agent.stoppingDistance = destinationAccuracy;
            unitComponent.SetFields(
                newUnitObject, unitDto.Type == UnitType.Warrior ? UnitTag.Warrior : UnitTag.EnemyWarrior, unitDto.Id);
            newEntity.AddUnitComponents(unitDto, newUnitObject);
        }
    }
}
