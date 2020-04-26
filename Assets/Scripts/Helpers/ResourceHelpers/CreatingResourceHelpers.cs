using System;
using System.Collections.Generic;
using Components;
using Components.Zavod;
using Leopotam.Ecs;
using Models;
using ServerCommunication;
using UnityEngine;

namespace Systems
{
    public class CreatingResourceHelpers
    {
        private static float xZavodScale = MapBuildingsPrefabsHolder.ZavodPrefab.transform.lossyScale.x * 90;
        private static float zZavodScale = MapBuildingsPrefabsHolder.ZavodPrefab.transform.lossyScale.z * 90;
        private static float bagSpawnersCount = 18;
        
        public static void CreateAddingMoneyBagEventOnZavod(EcsEntity zavod)
        {
            var zavodComponent = zavod.Get<ZavodComponent>();
            var resourceComponent = zavod.Get<ResourceGeneratorComponent>();
            resourceComponent.LastGeneratedMoneyTime = Time.time;
            var createMoneyBagEvent = zavod.Set<CreateResourceEvent>();
            createMoneyBagEvent.Count = resourceComponent.GenerateMoneyCount;
            var randAngle = UnityEngine.Random.Range(0, 360 / bagSpawnersCount) * bagSpawnersCount;
            createMoneyBagEvent.Position = zavodComponent.Position
                                           + new UnityEngine.Vector3(xZavodScale * Mathf.Sin(randAngle), 0, zZavodScale * Mathf.Cos(randAngle));
            createMoneyBagEvent.Tag = ResourceTag.Money;
            createMoneyBagEvent.Id = Guid.NewGuid();
            var bagDto = new BagDto()
            {
                Id = createMoneyBagEvent.Id,
                GoldCount = createMoneyBagEvent.Count,
                Position = createMoneyBagEvent.Position.ToModelsVector()
            };

            ServerClient.Communication.ClientInfoReceiver.ToServerCreateBag.Add(createMoneyBagEvent.Id, bagDto);
        }
    }
}