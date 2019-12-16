using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class AttackHelper
    {
        public static bool IsOnAttackRange(Vector3 attackingUnitPosition, Vector3 targetUnitPosition, float attackRange)
        {
            return Vector3.Distance(attackingUnitPosition, targetUnitPosition) <= attackRange;
        }

        public static bool IsNotOnCooldown(AttackComponent attackComponent)
        {
            return Time.time - attackComponent.LastAttackTime >= attackComponent.AttackDelay;
        }
        
        public static bool CanAttack(EcsEntity attackingUnit, EcsEntity targetUnit)
        {
            var attackComponent = attackingUnit.Get<AttackComponent>();
            var attackingPosition = attackingUnit.Get<UnitComponent>().Object.transform.position;
            var targetPosition = targetUnit.Get<UnitComponent>().Object.transform.position;
            return CanAttack(attackComponent, attackingPosition, targetPosition);
        }

        public static bool CanAttack(
            AttackComponent attackingUnitComponent,
            Vector3 attackingPosition,
            Vector3 targetPosition)
        {
            return IsNotOnCooldown(attackingUnitComponent)
                   && IsOnAttackRange(attackingPosition, targetPosition, attackingUnitComponent.AttackRange);
        }

        public static void CreateAttackEvent(EcsEntity attackingUnitEntity, EcsEntity targetUnitEntity)
        {
            var attackEvent = attackingUnitEntity.Set<AttackEvent>();
            attackEvent.TargetHealthComponent = targetUnitEntity.Get<HealthComponent>();
            attackEvent.TargetPosition = targetUnitEntity.Get<UnitComponent>().Object.transform.position;
            attackEvent.TargetGuid = targetUnitEntity.Get<UnitComponent>().Guid;
        }

        public static void CreateAttackEvent(
            EcsEntity attackingUnitEntity,
            HealthComponent targetHealthComponent,
            Vector3 targetPosition)
        {
            var attackEvent = attackingUnitEntity.Set<AttackEvent>();
            attackEvent.TargetHealthComponent = targetHealthComponent;
            attackEvent.TargetPosition = targetPosition;
        }
    }
}