using System.Linq;
using Components;
using Components.Health;
using Leopotam.Ecs;
using UnityEngine;

public class UnitChangeHealthSystem: IEcsRunSystem
{
    private readonly EcsFilter<HealthChangingEvent> changingHpUnits;
    private readonly EcsFilter<DestroyEvent> destroyingUnits;

    public void Run() => ChangeHp();

    private void ChangeHp()
    {
        var changingHpUnitsEntities = changingHpUnits.Entities
            .Take(changingHpUnits.GetEntitiesCount());
        foreach (var unit in changingHpUnitsEntities)
        {
            var unitHealthComponent = unit.Get<HealthComponent>();
            unitHealthComponent.CurrentHp = unit.Get<HealthChangingEvent>().NewHp;
            unit.Unset<HealthChangingEvent>();
            
            if (unitHealthComponent.CurrentHp <= 0)
            {
                unit.Set<DestroyEvent>().Object = unit.Get<UnitComponent>().Object;
                return;
            }
        }
    }
}