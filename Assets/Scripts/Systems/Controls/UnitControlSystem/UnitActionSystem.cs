using System.Collections.Generic;
using System.Linq;
using Components;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace Systems
{
    public class UnitActionSystem : IEcsSystem
    {
        private const float inertiaEliminatorFactor = 3;

        public static void Attack(EcsEntity attackingUnit, EcsEntity targetUnit)
        {
            var attackComponent = attackingUnit.Get<AttackComponent>();
            var targetHealthComponent = targetUnit.Get<HealthComponent>();
            if (!CanAttack(attackingUnit, targetUnit))
                return;
            targetHealthComponent.CurrentHp -= attackComponent.AttackDamage;
            attackComponent.LastAttackTime = Time.time;
        }
        
        public static void UpdateTargetForUnit(EcsEntity unitEntity, Vector3 targetPosition)
        {
            var agent = unitEntity.Get<UnitComponent>().Object.GetComponent<NavMeshAgent>();
            var movementComponent = unitEntity.Get<MovementComponent>();
            agent.SetDestination(targetPosition);
            agent.speed = movementComponent.MoveSpeed;
            agent.acceleration = movementComponent.MoveSpeed * inertiaEliminatorFactor;
        }

        public static void UpdateTargetForUnits(IEnumerable<EcsEntity> units, Vector3 targetPosition)
        {
            var existedUnits = units.Where(u => !u.IsNull() && u.IsAlive());
            foreach (var unit in existedUnits)
                UpdateTargetForUnit(unit, targetPosition);
        }

        private static bool CanAttack(EcsEntity attackingUnit, EcsEntity targetUnit)
        {
            var attackComponent = attackingUnit.Get<AttackComponent>();
            var attackingPosition = attackingUnit.Get<UnitComponent>().Object.transform.position;
            var targetPosition = targetUnit.Get<UnitComponent>().Object.transform.position;
            return IsNotOnCooldown(attackComponent)
                   && IsOnAttackRange(attackingPosition, targetPosition, attackComponent.AttackRange);
        }

        private static bool IsOnAttackRange(Vector3 attackingUnitPosition, Vector3 targetUnitPosition, float attackRange)
        {
            return Vector3.Distance(attackingUnitPosition, targetUnitPosition) <= attackRange;
        }

        private static bool IsNotOnCooldown(AttackComponent attackComponent)
        {
            return Time.time - attackComponent.LastAttackTime >= attackComponent.AttackDelay;
        }
    }
}