using System;
using System.Threading.Tasks;
using Components;
using Components.Buildings;
using Components.Tags.Buildings;
using Leopotam.Ecs;
using Models;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public static class GameObjectExtensions
    {
        private const float minimumHeight = 2.5f;
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

        public static void AddNewBuildingEntityFromBuildingDbo(
            this GameObject building,
            EcsWorld ecsWorld,
            Canvas canvas,
            ServerBuildingDto buildingDto,
            BuildingTag buildingType)
        {
            var newEntity = ecsWorld.NewEntityWith<BuildingComponent>(out var buildingComponent);
            buildingComponent.SetFields(
                building,
                buildingType,
                buildingDto.Id, 
                GuiHelper.InstantiateAllButtons(canvas, ecsWorld), ecsWorld);
            newEntity.AddUnitComponents(buildingDto);
        }

        public static async Task DestroyObjectWithDelay(this GameObject obj, int waitForMilliseconds = 500)
        {
            if (obj == null)
                return;
            await Task.Delay(waitForMilliseconds);
            Object.Destroy(obj);
        }

        private static GameObject InstantiateNewObject(this GameObject prefab, Vector3 position, Quaternion quaternion)
        {
            var minimumHeightPosition = new Vector3(position.x, Math.Min(position.y, minimumHeight), position.z);
            return Object.Instantiate(prefab, minimumHeightPosition, quaternion);
        }
        
        private static GameObject InstantiateNewObject(this GameObject prefab, Models.Vector3 position, Quaternion quaternion)
        {            
            var unityPosition = new Vector3(position.X, position.Y, position.Z);
            return InstantiateNewObject(prefab, unityPosition, quaternion);
        }

        public static Vector3 GetPosition(this GameObject someObject) => someObject.transform.position;
    }
}
