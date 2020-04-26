using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using Components.Zavod;
using Components;
using ServerCommunication;
using Models;

namespace Systems.Communication
{
    public class BagsCreateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ResourceComponent> resourcesFilter = null;
        private readonly EcsWorld world = null;

        public void Run()
        {
            var resources = resourcesFilter.Entities.Where(e => e.IsNotNullAndAlive());

            foreach (var serverBag in ServerClient.Communication.InGameInfo.Bags.Values)
            {
                if (resources
                    .Select(b => b.Get<ResourceComponent>())
                    .All(r => r.Guid != serverBag.Id))
                    CreateClientBag(serverBag);
            }
        }

        private void CreateClientBag(BagDto serverBag)
        {
            world.NewEntityWith(out CreateResourceEvent resEvent);
            resEvent.Count = serverBag.GoldCount;
            resEvent.Tag = ResourceTag.Money;
            resEvent.Position = serverBag.Position.ToUnityVector();
            resEvent.Id = serverBag.Id;
        }
    }
}
