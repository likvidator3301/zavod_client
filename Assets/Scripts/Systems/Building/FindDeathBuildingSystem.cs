using Components;
using Components.Health;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems
{
    class FindDeathBuildingSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingComponent> buildings = null;
        private readonly EcsWorld world = null;

        public void Run()
        {
            var buildingEntities = buildings.Entities
            .Where(u => u.IsNotNullAndAlive());

            foreach (var building in buildingEntities)
            {
                var buildingHealthComponent = building.Get<HealthComponent>();
                if (buildingHealthComponent.CurrentHp <= 0)
                {
                    if (building.Get<BuildingComponent>().Tag == BuildingTag.Base)
                    {
                        world.NewEntityWith(out EndGameEvent endEvent);
                        endEvent.IsWin = building.Get<EnemyBuildingComponent>() != null;
                    }

                    building.Set<DestroyEvent>().Object = building.Get<BuildingComponent>().Object;
                    return;
                }
            }
        }
    }
}
