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
        private readonly EcsFilter<BuildingComponent> changingHpBuildigs = null;

        public void Run()
        {
            var changingHpUnitsEntities = changingHpBuildigs.Entities
            .Where(u => u.IsNotNullAndAlive());

            foreach (var unit in changingHpUnitsEntities)
            {
                var unitHealthComponent = unit.Get<HealthComponent>();
                if (unitHealthComponent.CurrentHp <= 0)
                {
                    unit.Set<DestroyEvent>().Object = unit.Get<BuildingComponent>().Object;
                    return;
                }
            }
        }
    }
}
