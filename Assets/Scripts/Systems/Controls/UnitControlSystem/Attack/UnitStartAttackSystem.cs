using System.Linq;
using Components;
using Components.Attack;
using Leopotam.Ecs;

public class UnitStartAttackSystem: IEcsRunSystem
{
    private readonly EcsFilter<StartAttackingEvent, AttackComponent> startAttackingUnits;
    
    public void Run() => StartAttack();
    
    private void StartAttack()
    {
        var startAttackingUnitsEntities = startAttackingUnits.Entities
            .Take(startAttackingUnits.GetEntitiesCount());
        foreach (var unit in startAttackingUnitsEntities)
        {
            var startAttackingEvent = unit.Get<StartAttackingEvent>();
            //TODO: Entity already in filter
            unit.Set<AttackingComponent>().TargetEntity = startAttackingEvent.TargetEntity;
        
            unit.Unset<StartAttackingEvent>();
        }
    }
}