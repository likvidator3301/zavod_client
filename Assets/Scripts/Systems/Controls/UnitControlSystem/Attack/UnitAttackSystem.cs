using System.Linq;
using Systems;
using Components;
using Components.Attack;
using Components.Follow;
using Leopotam.Ecs;

public class UnitAttackSystem: IEcsRunSystem
{
    private readonly EcsFilter<AttackingComponent, AttackingComponent> attackingUnits;
    
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
            var targetMovementComponent = attackingComponent.TargetEntity.Get<MovementComponent>();
            var targetHealthComponent = attackingComponent.TargetEntity.Get<HealthComponent>();
            var targetUnit = attackingComponent.TargetEntity;
            
            if (!targetUnit.IsNotNullAndAlive() || unit.Get<FollowingComponent>() == null)
            {
                //TODO: Entity not in filter error
                AttackHelper.StopAttack(unit);
            }
            else if (AttackHelper.CanAttack(unitAttackComponent, unitMovementComponent, targetMovementComponent))
            {
                var newHp = targetHealthComponent.CurrentHp - unitAttackComponent.AttackDamage;
                HealthHelper.CreateChangeHpEvent(targetUnit, newHp);
            }
        }
    }
}