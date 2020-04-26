using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using Systems;
using Components;
using ServerCommunication;
using Models;
using Components.Health;
using UnityEngine;

namespace Systems.Communication
{
    public class BagsRemoveSystem : IEcsRunSystem
    {
        private readonly EcsWorld world = null;
        private readonly EcsFilter<ResourceComponent> resourceFilter = null;

        public void Run()
        {
            var resources = resourceFilter.Entities.Where(e => e.IsNotNullAndAlive());

            foreach (var res in resources)
            {
                var bag = res.Get<ResourceComponent>();

                if (!ServerClient.Communication.InGameInfo.Bags.ContainsKey(bag.Guid))
                {
                    GameObject.Destroy(bag.Object);
                    res.Destroy();
                }
            }
        }
    }
}
