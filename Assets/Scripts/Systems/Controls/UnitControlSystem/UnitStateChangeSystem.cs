using System;
using System.Linq;
using Components;
using Leopotam.Ecs;
using Object = UnityEngine.Object;

namespace Systems
{
    public class UnitStateChangeSystem : IEcsRunSystem
    {
        private EcsWorld ecsWorld;
        private PlayerComponent player;
        private EcsFilter<UnitComponent> units;

        public void Run()
        {
            DestroyDeadUnits();
        }

        private void DestroyDeadUnits()
        {
            foreach (var unitEntity in units.Entities
                .Where(u => !u.IsNull() && u.IsAlive() 
                                        && u.Get<HealthComponent>().CurrentHp <= 0))
            {
                Object.Destroy(unitEntity.Get<UnitComponent>().Object);
                unitEntity.Destroy();
            }
        }
    }
}