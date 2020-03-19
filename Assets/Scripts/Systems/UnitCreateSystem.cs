using System;
using System.Threading.Tasks;
using Leopotam.Ecs;
using Components;
using Models;
using Vector3 = UnityEngine.Vector3;
using System.Linq;

namespace Systems
{
    using Vector3 = Models.Vector3;

    public class UnitCreateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitCreateEvent> unitEvents = null;
        private readonly GameDefinitions gameDefinitions = null;
        private readonly EcsWorld world = null;

        public void Run() => CreateUnits();

        private async Task CreateUnits()
        {
            foreach (var unitEvent in unitEvents.Entities.Where(e => e.IsNotNullAndAlive())) 
            {
                var unitComponent = unitEvent.Get<UnitCreateEvent>();
                var newPosition = new Vector3(unitComponent.Position.x + 5,
                    unitComponent.Position.y,
                    unitComponent.Position.z);
                var newUnitDto = new CreateUnitDto
                {
                    Position = newPosition,
                    UnitType = unitComponent.UnitTag == UnitTag.Warrior
                        ? UnitType.Warrior
                        : UnitType.Chelovechik
                };

                var newUnit = await ServerCommunication.ServerClient.Client.Unit.CreateUnit(newUnitDto);
                UnitsPrefabsHolder.WarriorPrefab.AddNewUnitEntityFromUnitDbo(world, newUnit);
                unitEvent.Destroy();
            }
        }
    }
}
