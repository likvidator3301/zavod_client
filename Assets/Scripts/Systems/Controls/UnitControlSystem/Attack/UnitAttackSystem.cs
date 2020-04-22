using System.Linq;
using Systems;
using Components;
using Components.Attack;
using Components.Follow;
using Leopotam.Ecs;
using ServerCommunication;
using System;

public class UnitAttackSystem: IEcsRunSystem
{
    private readonly EcsFilter<AttackingComponent, AttackComponent, MovementComponent> attackingUnits;
    
    public void Run() => Attack();

    private void Attack()
    {
        var attackingUnitsEntities = attackingUnits.Entities
            .Where(u => u.IsNotNullAndAlive())
            .Take(attackingUnits.GetEntitiesCount());

        foreach (var unit in attackingUnitsEntities)
        {
            var attackingComponent = unit.Get<AttackingComponent>();
            var unitAttackComponent = unit.Get<AttackComponent>();
            var unitMovementComponent = unit.Get<MovementComponent>();
            
            var targetUnit = attackingComponent.TargetEntity;
            if (!targetUnit.IsNotNullAndAlive())
            {
                AttackHelper.StopAttack(unit);
                return;
            }

            var targetMovementComponent = targetUnit.Get<MovementComponent>();
            var targetHealthComponent = targetUnit.Get<HealthComponent>();
            
            if (targetMovementComponent == null || !targetMovementComponent.IsObjectAlive)
            {
                AttackHelper.StopAttack(unit);
                return;
            }
            
            
            if (AttackHelper.CanAttack(unitAttackComponent, unitMovementComponent, targetMovementComponent))
            {
                var newHp = targetHealthComponent.CurrentHp - unitAttackComponent.AttackDamage;
                HealthHelper.CreateChangeHpEvent(targetUnit, newHp);
                unitAttackComponent.LastAttackTime = DateTime.Now;

                ServerClient.Communication.AttackSender.attacks.Add(
                    new AttackInfo(unit.Get<UnitComponent>().Guid, 
                                   attackingComponent.TargetEntity.Get<UnitComponent>().Guid, (int)unitAttackComponent.AttackDamage));
            }
        }
    }
}