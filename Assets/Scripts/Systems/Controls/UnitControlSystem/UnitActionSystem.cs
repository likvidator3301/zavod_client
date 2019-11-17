using System.Collections.Generic;
using Components;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace Systems
{
    public class UnitActionSystem : IEcsSystem
    {
        private const float inertiaEliminatorFactor = 3;

        public static void Attack(UnitComponent attackingUnit, UnitComponent targetUnit)
        {
            var attackComponent = attackingUnit.Object.GetComponent<AttackComponent>();
            var targetHealthComponent = targetUnit.Object.GetComponent<HealthComponent>();
            if (!CanAttack(attackingUnit, targetUnit))
                return;
            targetHealthComponent.CurrentHp -= attackComponent.AttackDamage;
            attackComponent.LastAttackTime = Time.time;
        }

        public static void UpdateTargets(Vector3 targetPosition, List<UnitComponent> units)
        {
            foreach (var unit in units)
                UpdateTarget(targetPosition, unit);
        }

        public static void UpdateTarget(Vector3 targetPosition, UnitComponent unit)
        {
            var agent = unit.Object.GetComponent<NavMeshAgent>();
            var movementComponent = unit.Object.GetComponent<MovementComponent>();
            agent.SetDestination(targetPosition);
            agent.speed = movementComponent.MoveSpeed;
            agent.acceleration = movementComponent.MoveSpeed * inertiaEliminatorFactor;
        }

        private static bool CanAttack(UnitComponent attackingUnit, UnitComponent targetUnit)
        {
            var attackComponent = attackingUnit.Object.GetComponent<AttackComponent>();
            var attackingPosition = attackingUnit.Object.transform.position;
            var targetPosition = targetUnit.Object.transform.position;
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