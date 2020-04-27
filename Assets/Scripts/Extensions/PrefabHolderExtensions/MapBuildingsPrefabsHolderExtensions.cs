using System;
using Components.Base;
using Components.Zavod;
using Extensions;
using Leopotam.Ecs;
using UnityEngine;
using Components;

namespace Systems
{
    public static class MapBuildingsPrefabsHolderExtensions
    {
        private static readonly Vector3 buildingRotation = new Vector3(0, 90, 0);
        
        public static void AddNewZavodEntityOnPosition(
            this GameObject prefab,
            EcsWorld world,
            Vector3 position)
        {
            var newZavodObject = prefab.InstantiateNewObject(position, Quaternion.Euler(buildingRotation));
            world.NewEntityWith<ZavodComponent, ResourceGeneratorComponent>(
                out var zavodComponent,
                out var resourceGeneratorComponent);
            zavodComponent.Position = position;
            zavodComponent.Object = newZavodObject;
            zavodComponent.Guid = Guid.NewGuid();
            zavodComponent.Tag = MapBuildingTag.Zavod;
        }

        public static void AddNewBaseEntityOnPosition(
            this GameObject prefab,
            EcsWorld world,
            Vector3 position)
        {
            var newBaseObject = prefab.InstantiateNewObject(position, Quaternion.Euler(buildingRotation));
            var baseEntity = world.NewEntityWith<BaseComponent>(
                out var baseComponent);
        }
    }
}