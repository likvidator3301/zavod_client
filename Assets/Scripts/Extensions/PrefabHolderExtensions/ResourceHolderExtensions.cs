using System;
using Components;
using Extensions;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public static class MoneyBagHolderExtensions
    {
        private static readonly int moneyCount = 50;
        private static readonly int semkiCount = 10;
        
        public static void AddResourceEntityOnPosition(
            this GameObject prefab,
            EcsWorld world,
            Vector3 position,
            ResourceTag tag,
            int resourceCount = -1)
        {
            var newMoneyBagObject = prefab.InstantiateNewObject(position, Quaternion.identity);
            world.NewEntityWith<ResourceComponent>(out var resourceComponent);
            
            resourceComponent.Object = newMoneyBagObject;
            resourceComponent.Position = position;
            resourceComponent.Tag = tag;
            resourceComponent.Guid = Guid.NewGuid();
            if (resourceCount == -1)
                resourceComponent.ResourceCount = tag == ResourceTag.Money ? moneyCount : semkiCount;
            else
                resourceComponent.ResourceCount = resourceCount;
        }
    }
}