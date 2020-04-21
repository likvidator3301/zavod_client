// using System;
// using Components;
// using Extensions;
// using Leopotam.Ecs;
// using Models;
// using UnityEngine;
// using UnityEngine.AI;
// using Vector3 = UnityEngine.Vector3;
//
// namespace Systems
// {
//     public static class UnitPrefabsHolderExtensions
//     {
//         private const float destinationAccuracy = 0.5f;
//         
//         //TODO: find healthBar in unitObject
//         public static void AddNewWarriorEntityFromUnitDbo(
//             this GameObject prefab, EcsWorld world, ServerUnitDto unitDto)
//         {
//             var newUnitObject = prefab.InstantiateNewObject(unitDto.Position, Quaternion.identity);
//             var newEntity = world.NewEntityWith<UnitComponent, UnitAnimationComponent, NavMeshComponent, HealthBarComponent>(
//                 out var unitComponent,
//                 out var unitAnimationComponent,
//                 out var navMeshAgentComponent,
//                 out var healthBarComponent);
//             
//             //healthBarComponent = ...
//             unitAnimationComponent.Animator = newUnitObject.GetComponent<Animator>();
//             navMeshAgentComponent.Agent = newUnitObject.GetComponent<NavMeshAgent>();
//             navMeshAgentComponent.Agent.stoppingDistance = destinationAccuracy;
//             unitComponent.SetFields(
//                 newUnitObject, unitDto.Type == UnitType.Warrior ? UnitTag.Warrior : UnitTag.EnemyWarrior, unitDto.Id);
//             newEntity.AddWarriorComponents(unitDto, newUnitObject);
//         }
//
//         public static void AddNewDeliverEntityOnPosition(
//             this GameObject prefab, EcsWorld world, Vector3 position, Guid guid, UnitTag tag = UnitTag.Deliver)
//         {
//             var newUnitObject = prefab.InstantiateNewObject(position, Quaternion.identity);
//             var newEntity = world.NewEntityWith<UnitComponent, NavMeshComponent, HealthBarComponent, ResourceDeliverComponent>(
//                 out var unitComponent,
//                 out var navMeshAgentComponent,
//                 out var healthBarComponent,
//                 out var deliverComponent);
//             
//             //healthBarComponent = ...
//             //unitAnimationComponent.Animator = newUnitObject.GetComponent<Animator>();
//             navMeshAgentComponent.Agent = newUnitObject.GetComponent<NavMeshAgent>();
//             navMeshAgentComponent.Agent.stoppingDistance = destinationAccuracy;
//             unitComponent.SetFields(
//                 newUnitObject, tag, guid);
//             newEntity.AddDeliverComponents(position, newUnitObject);
//             deliverComponent.MaxResourcesTakenCount = 3;
//         }
//     }
// }
