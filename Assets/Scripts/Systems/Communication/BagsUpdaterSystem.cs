using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using Systems;
using Components;
using ServerCommunication;
using Models;
using Components.Health;

namespace Systems.Communication
{
    public class BagsUpdaterSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ResourceComponent> resourceFilter = null;

        public void Run()
        {
            var resources = resourceFilter.Entities.Where(e => e.IsNotNullAndAlive());

            foreach (var res in resources)
            {
                var bag = res.Get<ResourceComponent>();

                if (!ServerClient.Communication.InGameInfo.Bags.ContainsKey(bag.Guid)
                    && !ServerClient.Communication.ClientInfoReceiver.ToServerCreateBag.ContainsKey(bag.Guid))
                {
                    var destroy = res.Set<DestroyEvent>();
                    destroy.Object = bag.Object;
                }
            }
        }
    }
}
