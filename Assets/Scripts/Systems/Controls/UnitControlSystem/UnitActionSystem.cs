using System.Linq;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace Systems
{
    public class UnitActionSystem : IEcsRunSystem
    {
        private ServerIntegration.ServerIntegration serverIntegration;
        private const float inertiaEliminatorFactor = 3;
        private EcsWorld ecsWorld;
        private EcsFilter<UnitComponent> units;
        private EcsFilter<AttackEvent> attackEvents;
        private EcsFilter<MoveEvent> moveEvents;
        private EcsFilter<FollowEvent> followEvents;

        public void Run()
        {
            UnsetFollowEventsIfIsMovingToTarget();
            UnsetFollowEventsIfCanAttackTarget();
            CreateAttackEventsIfCanAttackAndNotMoving();
            
            MoveUnitsToTarget();
            FollowUnitsToTarget();
            AttackTargets();
        }
        
        private void AttackTargets()
        {
            var attackEventEntities = attackEvents.Entities
                .Where(e => e.IsNotNullAndAlive()
                            && e.Get<DieEvent>() == null
                            && e.Get<MoveEvent>() == null
                            && e.Get<AttackEvent>() != null);
            foreach (var attackEventEntity in attackEventEntities)
                Attack(attackEventEntity);
        }

        private void Attack(EcsEntity attackingUnitEntity)
        {
            var attackEvent = attackingUnitEntity.Get<AttackEvent>();
            var targetGuid = attackEvent.TargetGuid;
            var attackingGuid = attackingUnitEntity.Get<UnitComponent>().Guid;
            var targetPosition = attackEvent.TargetPosition;
            var targetHealthComponent = attackEvent.TargetHealthComponent;
            var attackingUnitAttackComponent = attackingUnitEntity.Get<AttackComponent>();
            var attackingPosition = attackingUnitEntity.Get<UnitComponent>().Object.transform.position;
            if (!AttackHelper.CanAttack(attackingUnitAttackComponent, attackingPosition, targetPosition))
            {
                attackingUnitEntity.Unset<AttackEvent>();
                return;
            }
            
            UnitAnimationHelper.CreateAttackEvent(attackingUnitEntity);
            serverIntegration.client.Unit.AddUnitsToAttack(attackingGuid, targetGuid);

            targetHealthComponent.CurrentHp -= attackingUnitAttackComponent.AttackDamage;
            attackingUnitAttackComponent.LastAttackTime = Time.time;
        }

        private void MoveUnitsToTarget()
        {
            var moveEventEntities = moveEvents.Entities
                .Where(e => e.IsNotNullAndAlive() && e.Get<MoveEvent>() != null);
            foreach (var moveEventEntity in moveEventEntities)
            {
                var moveEvent = moveEventEntity.Get<MoveEvent>();
                UnitAnimationHelper.CreateMovingEvent(moveEventEntity);
                
                UpdateTargetForUnit(
                    moveEventEntity.Get<UnitComponent>().Agent, 
                    moveEventEntity.Get<MovementComponent>(),
                    moveEvent.TargetPosition);
                moveEventEntity.Unset<MoveEvent>();
            }
        }
        
        private void UpdateTargetForUnit(
            NavMeshAgent unitAgent,
            MovementComponent unitMovementComponent,
            Vector3 targetPosition)
        {
            unitAgent.SetDestination(targetPosition);
            unitAgent.speed = unitMovementComponent.MoveSpeed;
            unitAgent.acceleration = unitMovementComponent.MoveSpeed * inertiaEliminatorFactor;
        }
        
        private void FollowUnitsToTarget()
        {
            var followEventEntities = followEvents.Entities
                .Where(e => e.IsNotNullAndAlive() && e.Get<FollowEvent>() != null);
            foreach (var followEventEntity in followEventEntities)
            {
                var followEvent = followEventEntity.Get<FollowEvent>();
                var targetUnitComponent = followEvent.TargetUnitComponent;
                if (targetUnitComponent == null || targetUnitComponent.Object == null)
                {
                    followEventEntity.Unset<FollowEvent>();
                    continue;
                }

                UnitAnimationHelper.CreateMovingEvent(followEventEntity);
                
                var targetPosition = targetUnitComponent.Object.transform.position;
                var unitAgent = followEventEntity.Get<UnitComponent>().Agent;
                var unitMovementComponent = followEventEntity.Get<MovementComponent>();
                UpdateTargetForUnit(unitAgent, unitMovementComponent, targetPosition);
            }
        }

        private void UnsetFollowEventsIfIsMovingToTarget()
        {
            var movingFollowEventEntities = followEvents.Entities
                .Where(e => e.IsNotNullAndAlive()
                            && e.Get<MoveEvent>() != null);
            foreach (var followEventEntity in movingFollowEventEntities)
                followEventEntity.Unset<FollowEvent>();
        }

        private void UnsetFollowEventsIfCanAttackTarget()
        {
            var followEventEntities = followEvents.Entities
                .Where(e => e.IsNotNullAndAlive() && e.Get<AttackComponent>() != null);
            foreach (var followEventEntity in followEventEntities)
            {
                var followEvent = followEventEntity.Get<FollowEvent>();
                if (followEvent == null || followEvent.TargetUnitComponent == null)
                {
                    followEventEntity.Unset<FollowEvent>();
                    continue;
                }

                var targetUnitComponent = followEvent.TargetUnitComponent;
                var unitAttackComponent = followEventEntity.Get<AttackComponent>();
                var unitPosition = followEventEntity.Get<UnitComponent>().Object.transform.position;
                var targetPosition = targetUnitComponent.Object.transform.position;
                if (targetUnitComponent.Tag == UnitTag.EnemyWarrior
                    && AttackHelper.CanAttack(unitAttackComponent, unitPosition, targetPosition)
                    || targetUnitComponent.Tag == UnitTag.Warrior)
                {
                    var movingObjectAgent = followEventEntity.Get<UnitComponent>().Agent;
                    movingObjectAgent.SetDestination(unitPosition);
                    followEventEntity.Unset<FollowEvent>();
                }
            }
        }
        
        private void CreateAttackEventsIfCanAttackAndNotMoving()
        {
            var allyUnits = units.Entities
                .Where(u => u.IsNotNullAndAlive() && u.Get<UnitComponent>().Tag == UnitTag.Warrior);
            var enemyUnits = units.Entities
                .Where(u => u.IsNotNullAndAlive() && u.Get<UnitComponent>().Tag == UnitTag.EnemyWarrior);
            
            foreach (var attackingEntity in allyUnits)
            {
                var targetEntity = enemyUnits
                    .FirstOrDefault(enemyEntity => CanAttackAndNotAttackingNow(attackingEntity, enemyEntity));
                if (targetEntity != default)
                    AttackHelper.CreateAttackEvent(attackingEntity, targetEntity);
            }
        }

        private bool CanAttackAndNotAttackingNow(EcsEntity allyUnit, EcsEntity enemyUnit)
        {
            var haveAttackEvent = allyUnit.Get<AttackEvent>() != null;
            
            return !haveAttackEvent && AttackHelper.CanAttack(allyUnit, enemyUnit);
        }
    }
}
