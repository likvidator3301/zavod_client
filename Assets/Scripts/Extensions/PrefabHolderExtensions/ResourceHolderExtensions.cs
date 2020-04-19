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
        private static readonly float minHeight = 0f;
        
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

        public static void AddResourceEntityFromResourceComponent(
            this GameObject prefab,
            EcsWorld world,
            ResourceComponent resourceComponent)
        {
            var alignedPosition = new Vector3(
                resourceComponent.Position.x,
                minHeight,
                resourceComponent.Position.z);
            var newMoneyBagObject = prefab.InstantiateNewObject(alignedPosition, Quaternion.identity);
            world.NewEntityWith<ResourceComponent>(out var newResourceComponent);

            newResourceComponent.Object = newMoneyBagObject;
            newResourceComponent.Position = alignedPosition;
            newResourceComponent.Tag = resourceComponent.Tag;
            newResourceComponent.Guid = resourceComponent.Guid;
            newResourceComponent.ResourceCount = resourceComponent.ResourceCount;
        }
    }
}