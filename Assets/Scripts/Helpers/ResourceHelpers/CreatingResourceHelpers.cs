using System.Collections.Generic;
using Components;
using Components.Zavod;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class CreatingResourceHelpers
    {
        private static float xZavodScale = MapBuildingsPrefabsHolder.ZavodPrefab.transform.lossyScale.x * 65;
        private static float zZavodScale = MapBuildingsPrefabsHolder.ZavodPrefab.transform.lossyScale.z * 65;
        private static List<Vector3> randomFactor = new List<Vector3>()
        {
            new Vector3(xZavodScale, 0, zZavodScale),
            new Vector3(-xZavodScale, 0, zZavodScale),
            new Vector3(xZavodScale, 0, -zZavodScale),
            new Vector3(-xZavodScale, 0, -zZavodScale)
        };
        
        public static void CreateAddingMoneyBagEventOnZavod(EcsEntity zavod)
        {
            var zavodComponent = zavod.Get<ZavodComponent>();
            var resourceComponent = zavod.Get<ResourceGeneratorComponent>();
            resourceComponent.LastGeneratedMoneyTime = Time.time;
            var createMoneyBagEvent = zavod.Set<CreateResourceEvent>();
            createMoneyBagEvent.Count = resourceComponent.GenerateMoneyCount;
            createMoneyBagEvent.Position = zavodComponent.Position
                                           + randomFactor[Random.Range(0, 4)];
            createMoneyBagEvent.Tag = ResourceTag.Money;
        }
    }
}