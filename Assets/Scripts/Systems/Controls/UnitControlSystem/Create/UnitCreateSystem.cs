using System;
using System.Threading.Tasks;
using Leopotam.Ecs;
using Components;
using Models;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;

namespace Systems
{
    using Vector3 = Models.Vector3;

    public class UnitCreateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitCreateEvent> unitEvents = null;
        private readonly EcsFilter<UnitAssetsComponent> unitAssets = null;
        private readonly GameDefinitions gameDefinitions = null;
        private readonly EcsWorld world = null;

        public void Run() => CreateUnits();

        private async Task CreateUnits()
        {
            for (var i = 0; i < unitEvents.GetEntitiesCount(); i++)
            {
                var newPosition = new UnityEngine.Vector3(unitEvents.Get1[i].Position.x + 5,
                    unitEvents.Get1[i].Position.y,
                    unitEvents.Get1[i].Position.z);

                var unitInstance = GameObject.Instantiate(unitAssets.Get1[0].assetsByTag[unitEvents.Get1[i].UnitTag.ToString()]);
                unitInstance.transform.position = newPosition;


                var unitEntity = world.NewEntityWith(out UnitComponent unit);
                unit.Guid = new Guid();
                unit.Object = unitInstance;
                unit.Tag = unitEvents.Get1[i].UnitTag;
                unitEntity.AddWarriorComponents(unitInstance);

                unitEvents.Entities[i].Destroy();
            }
        }
    }
}