using System;
using Components;
using Leopotam.Ecs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems
{
    public static class GameObjectExtensions
    {
        private const float minimumHeight = 2.5f;
        
        public static void AddNewEntityOnPositionWithTag(this GameObject prefab, EcsWorld ecsWorld, Vector3 position, UnitTag unitTag)
        {
            var minimumHeightPosition = new Vector3(position.x, Math.Min(position.y, minimumHeight), position.z);
            ecsWorld.NewEntityWith<UnitComponent>(out var newUnit);
            var newUnitObject = Object.Instantiate(prefab, minimumHeightPosition, Quaternion.identity);
            newUnit.SetFields(newUnitObject, unitTag);
            newUnit.AddWarriorComponents();
        }
        
        public static void ChangeColorOn(this GameObject gameObject, Color color)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }
    }
}