using Components;
using Components.Attack;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class AttackHelper
    {
        public static void CreateAttackEvent(EcsEntity unit, EcsEntity targetUnit)
        {
            unit.Set<StartAttackEvent>().TargetEntity = targetUnit;
        }

        public static void StopAttack(EcsEntity unit)
        {
            unit.Unset<AttackingComponent>();
        }

        public static bool CanAttack(
            AttackComponent unitAttackComponent,
            MovementComponent unitMovementComponent,
            MovementComponent targetMovementComponent)
        {
            return unitAttackComponent.LastAttackTime + unitAttackComponent.AttackDelay <= Time.time
                && Vector3.Distance(
                    unitMovementComponent.CurrentPosition, 
                    targetMovementComponent.CurrentPosition) <= unitAttackComponent.AttackRange;
        }
    }
}