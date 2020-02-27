using System;
using System.Threading.Tasks;
using Components;
using Components.Buildings;
using Components.Tags.Buildings;
using Leopotam.Ecs;
using Models;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public static class GameObjectExtensions
    {
        private const float minimumHeight = 2.5f;
        
        public static void AddNewUnitEntityFromUnitDbo(
            this GameObject prefab, EcsWorld ecsWorld, ServerUnitDto unitDto)
        {
            var newUnitObject = InstantiateNewObject(prefab, unitDto.Position, Quaternion.identity);
            var newEntity = ecsWorld.NewEntityWith<UnitComponent>(out var unitComponent);
            unitComponent.SetFields(
                newUnitObject, unitDto.Type == UnitType.Warrior ? UnitTag.Warrior : UnitTag.EnemyWarrior, unitDto.Id);
            newEntity.AddComponents(unitDto);
        }

        public static void AddNewBuildingEntityFromBuildingDbo(
            this GameObject prefab,
            EcsWorld ecsWorld,
            Canvas canvas,
            ServerBuildingDto buildingDto,
            BuildingTag buildingType)
        {
            var newBuildingObject = InstantiateNewObject(prefab, buildingDto.Position, Quaternion.identity);
            var newEntity = ecsWorld.NewEntityWith<BuildingComponent>(out var buildingComponent);
            buildingComponent.SetFields(newBuildingObject, buildingType, buildingDto.Id, canvas, ecsWorld);
            newEntity.AddComponents(buildingDto);
        }

        public static async Task DestroyObjectWithDelay(this GameObject obj, int waitForMilliseconds = 500)
        {
            if (obj == null)
                return;
            await Task.Delay(waitForMilliseconds);
            Object.Destroy(obj);
        }

        private static GameObject InstantiateNewObject(GameObject prefab, Vector3 position, Quaternion quaternion)
        {
            var minimumHeightPosition = new Vector3(position.x, Math.Min(position.y, minimumHeight), position.z);
            return Object.Instantiate(prefab, minimumHeightPosition, quaternion);
        }
        
        private static GameObject InstantiateNewObject(GameObject prefab, Models.Vector3 position, Quaternion quaternion)
        {            
            var unityPosition = new Vector3(position.X, position.Y, position.Z);
            return InstantiateNewObject(prefab, unityPosition, quaternion);
        }
    }
}
