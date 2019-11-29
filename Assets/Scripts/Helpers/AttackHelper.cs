using Components;
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
            return IsNotOnCooldown(attackComponent)
                   && IsOnAttackRange(attackingPosition, targetPosition, attackComponent.AttackRange);
        }
    }
}