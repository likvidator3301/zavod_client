using System.Linq;
using Systems;
using Components;
using Components.Attack;
using Components.Follow;
using Leopotam.Ecs;

public class UnitAttackSystem: IEcsRunSystem
{
    private readonly EcsFilter<AttackingComponent, AttackComponent, MovementComponent> attackingUnits;
    
    public void Run() => Attack();

    private void Attack()
    {
        var attackingUnitsEntities = attackingUnits.Entities
            .Take(attackingUnits.GetEntitiesCount());
        foreach (var unit in attackingUnitsEntities)
        {
            var attackingComponent = unit.Get<AttackingComponent>();
            var unitAttackComponent = unit.Get<AttackComponent>();
            var unitMovementComponent = unit.Get<MovementComponent>();
            
            var targetUnit = attackingComponent.TargetEntity;
            var targetMovementComponent = targetUnit.Get<MovementComponent>();
            var targetHealthComponent = targetUnit.Get<HealthComponent>();
            
            if (!targetUnit.IsNotNullAndAlive() || targetMovementComponent == null || !targetMovementComponent.IsObjectAlive)
            {
                AttackHelper.StopAttack(unit);
                return;
            }
            
            
            if (AttackHelper.CanAttack(unitAttackComponent, unitMovementComponent, targetMovementComponent))
            {
                var newHp = targetHealthComponent.CurrentHp - unitAttackComponent.AttackDamage;
                HealthHelper.CreateChangeHpEvent(targetUnit, newHp);
            }
        }
    }
}