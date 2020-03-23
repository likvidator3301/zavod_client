using System;
using Components.Zavod;
using Extensions;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public static class ZavodPrefabHolderExtensions
    {
        private static readonly Vector3 zavodRotation = new Vector3(-90, 0, 0);
        
        public static void AddNewZavodEntityOnPosition(
            this GameObject prefab,
            EcsWorld world,
            Vector3 position)
        {
            var newZavodObject = prefab.InstantiateNewObject(position, Quaternion.Euler(zavodRotation));
            world.NewEntityWith<ZavodComponent, ResourceGeneratorComponent>(
                out var zavodComponent,
                out var resourceGeneratorComponent);
            zavodComponent.Position = position;
            zavodComponent.Object = newZavodObject;
            zavodComponent.Guid = Guid.NewGuid();
            zavodComponent.Tag = MapBuildingTag.Zavod;
        }
    }
}