using System.Linq;
using Components;
using Components.Health;
using Leopotam.Ecs;
using Systems;

public class FindDeadUnitsSystem: IEcsRunSystem
{
    private readonly EcsFilter<UnitComponent> changingHpUnits;

    public void Run() => ChangeHp();

    private void ChangeHp()
    {
        var changingHpUnitsEntities = changingHpUnits.Entities
            .Where(u => u.IsNotNullAndAlive());

        foreach (var unit in changingHpUnitsEntities)
        {
            var unitHealthComponent = unit.Get<HealthComponent>();
            if (unitHealthComponent.CurrentHp <= 0)
            {
                unit.Set<DestroyEvent>().Object = unit.Get<UnitComponent>().Object;
                return;
            }
        }
    }
}