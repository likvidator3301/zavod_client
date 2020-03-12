using System.Linq;
using Components;
using Components.Health;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Controls.UnitControlSystem
{
    public class HealthSystem: IEcsRunSystem
    {
        private readonly EcsFilter<HealthChangingEvent> changingHpUnits;
        private readonly EcsFilter<DestroyEvent> destroyingUnits;
        
        public void Run()
        {
            
        }

        private void ChangeHp()
        {
            var changingHpUnitsEntities = changingHpUnits.Entities
                .Take(changingHpUnits.GetEntitiesCount());
            foreach (var unit in changingHpUnitsEntities)
            {
                var unitHealthComponent = unit.Get<HealthComponent>();
                Debug.Log($"Previous HP: {unitHealthComponent.CurrentHp}");
                unitHealthComponent.CurrentHp = unit.Get<HealthChangingEvent>().NewHp;
                Debug.Log($"New HP: {unitHealthComponent.CurrentHp}");
                
                unit.Unset<HealthChangingEvent>();
            }
        }

        private void DestroyUnit()
        {
            
        }
    }
}