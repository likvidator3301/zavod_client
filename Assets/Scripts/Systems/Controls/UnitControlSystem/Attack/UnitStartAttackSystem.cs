using System.Linq;
using Systems;
using Components;
using Components.Attack;
using Leopotam.Ecs;

public class UnitStartAttackSystem: IEcsRunSystem
{
    private readonly EcsFilter<StartAttackEvent, AttackComponent> startAttackingUnits;
    
    public void Run() => StartAttack();
    
    private void StartAttack()
    {
        var startAttackUnitsEntities = startAttackingUnits.Entities
            .Take(startAttackingUnits.GetEntitiesCount());
        foreach (var unit in startAttackUnitsEntities)
        {
            var startAttackEvent = unit.Get<StartAttackEvent>();
            unit.Set<AttackingComponent>().TargetEntity = startAttackEvent.TargetEntity;
        
            unit.Unset<StartAttackEvent>();
        }
    }
}