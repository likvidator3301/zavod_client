using System;
using System.Threading.Tasks;
using Components;
using Leopotam.Ecs;
using Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public static class GameObjectExtensions
    {
        private const float minimumHeight = 2.5f;
        
        public static void AddNewUnitEntityOnPositionWithTag(
            this GameObject prefab, EcsWorld ecsWorld, Vector3 position, UnitTag unitTag)
        {
            var minimumHeightPosition = new Vector3(position.x, Math.Min(position.y, minimumHeight), position.z);
            var newEntity = ecsWorld.NewEntityWith<UnitComponent>(out var unitComponent);
            var newUnitObject = Object.Instantiate(prefab, minimumHeightPosition, Quaternion.identity);
            unitComponent.SetFields(newUnitObject, unitTag);
            newEntity.AddWarriorComponents();
        }

        public static void AddNewUnitEntityOnPositionFromUnitDbo(
            this GameObject prefab, EcsWorld ecsWorld, Vector3 position, CreateUnitDto unitDto)
        {
            {
                var minimumHeightPosition = new Vector3(position.x, Math.Min(position.y, minimumHeight), position.z);
                var newEntity = ecsWorld.NewEntityWith<UnitComponent>(out var unitComponent);
                var newUnitObject = Object.Instantiate(prefab, minimumHeightPosition, Quaternion.identity);
                unitComponent.SetFields(
                    newUnitObject, unitDto.UnitType == UnitType.Warrior ? UnitTag.Warrior : UnitTag.EnemyWarrior);
                newEntity.AddWarriorComponents();
            }
        }

        public static async Task DestroyObjectWithDelay(this GameObject obj, int waitForMilliseconds = 500)
        {
            if (obj == null)
                return;
            await Task.Delay(waitForMilliseconds);
            Object.Destroy(obj);
        }
    }
}