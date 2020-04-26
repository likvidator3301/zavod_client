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
            var randAngle = UnityEngine.Random.Range(0, 360 / bagSpawnersCount) * bagSpawnersCount;
            var bagPosition = zavodComponent.Position
                                    + new UnityEngine.Vector3(xZavodScale * Mathf.Sin(randAngle), 0, zZavodScale * Mathf.Cos(randAngle));

            var bagDto = new BagDto()
            {
                Id = Guid.NewGuid(),
                GoldCount = resourceComponent.GenerateMoneyCount,
                Position = bagPosition.ToModelsVector()
            };

            ServerClient.Communication.ClientInfoReceiver.ToServerCreateBag.Add(bagDto.Id, bagDto);
        }
    }
}