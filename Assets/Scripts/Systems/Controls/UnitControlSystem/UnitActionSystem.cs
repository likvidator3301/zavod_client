using System.Linq;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UnitActionSystem : IEcsRunSystem
    {
        private const float inertiaEliminatorFactor = 3;
        private readonly EcsWorld ecsWorld = null;
        private readonly EcsFilter<UnitComponent> units = null;
        private readonly EcsFilter<AttackEvent> attackEvents = null;
        private readonly EcsFilter<MoveEvent> moveEvents = null;
        private readonly EcsFilter<FollowEvent> followEvents = null;

        public void Run()
        {
            DestroyFollowEventsIfIsMoveToTarget();
            DestroyFollowEventsIfCanAttackTarget();
            CreateAttackEventsIfCanAttackAndNotMoving();
            
            MoveUnitsToTarget();
            FollowUnitsToTarget();
            AttackTargets();
        }
        
        private void AttackTargets()
        {
            var attackEventEntities = attackEvents.Entities
                .Where(e => e.IsNotNullAndAlive()
                            && !e.Get<AttackEvent>().AttackingUnit.Get<UnitComponent>().Animator.GetBool(
                                UnitAnimationState.IsMoving.ToString())
                            && e.Get<AttackEvent>().AttackingUnit.Get<HealthComponent>().CurrentHp > 0);
            foreach (var attackEventEntity in attackEventEntities)
                Attack(attackEventEntity);
        }

        private void Attack(EcsEntity attackEventEntity)
        {
            var attackEvent = attackEventEntity.Get<AttackEvent>();
            var attackingUnit = attackEvent.AttackingUnit;
            var targetUnit = attackEvent.Target;
            var attackComponent = attackingUnit.Get<AttackComponent>();
            var targetHealthComponent = targetUnit.Get<HealthComponent>();
            if (!attackingUnit.IsNotNullAndAlive()
                || !targetUnit.IsNotNullAndAlive()
                || !AttackHelper.CanAttack(attackingUnit, targetUnit))
            {
                attackEventEntity.Destroy();
                return;
            }
            
            UnitAnimationHelper.CreateAttackEvent(ecsWorld, attackingUnit);

            targetHealthComponent.CurrentHp -= attackComponent.AttackDamage;
            attackComponent.LastAttackTime = Time.time;
            if (targetHealthComponent.CurrentHp <= 0)
            {
                ChangeStateHelper.CreateDieEvent(ecsWorld, targetUnit);
                attackEventEntity.Destroy();
            }
        }

        private void MoveUnitsToTarget()
        {
            var moveEventEntities = moveEvents.Entities
                .Where(e => e.IsNotNullAndAlive());
            foreach (var moveEventEntity in moveEventEntities)
            {
                var moveEvent = moveEventEntity.Get<MoveEvent>();
                UnitAnimationHelper.CreateMovingEvent(ecsWorld, moveEvent.MovingObject);
                
                UpdateTargetForUnit(moveEvent.MovingObject, moveEvent.NextPosition);
                moveEventEntity.Destroy();
            }
        }
        
        private void UpdateTargetForUnit(EcsEntity unitEntity, Vector3 targetPosition)
        {
            var agent = unitEntity.Get<UnitComponent>().Agent;
            var movementComponent = unitEntity.Get<MovementComponent>();
            agent.SetDestination(targetPosition);
            agent.speed = movementComponent.MoveSpeed;
            agent.acceleration = movementComponent.MoveSpeed * inertiaEliminatorFactor;
        }

        private void DestroyFollowEventsIfIsMoveToTarget()
        {
            var followEventEntities = followEvents.Entities
                .Where(e => e.IsNotNullAndAlive());
            var movingObjects = moveEvents.Entities
                .Where(e => e.IsNotNullAndAlive())
                .Select(e => e.Get<MoveEvent>().MovingObject);
            var toDestroyEvents = followEventEntities
                .Where(e => movingObjects.Contains(e.Get<FollowEvent>().MovingObject));
            
            foreach (var destroyingEvent in toDestroyEvents)
                destroyingEvent.Destroy();
        }

        private void DestroyFollowEventsIfCanAttackTarget()
        {
            var followEventEntites = followEvents.Entities
                .Where(e => e.IsNotNullAndAlive());
            foreach (var followEventEntity in followEventEntites)
            {
                var movingObject = followEventEntity.Get<FollowEvent>().MovingObject;
                var target = followEventEntity.Get<FollowEvent>().Target;
                if (!target.IsNotNullAndAlive())
                {
                    followEventEntity.Destroy();
                    continue;
                }
                
                if (target.Get<UnitComponent>().Tag == UnitTag.EnemyWarrior
                    && AttackHelper.CanAttack(movingObject, target))
                {
                    var movingObjectAgent = movingObject.Get<UnitComponent>().Agent;
                    movingObjectAgent.SetDestination(movingObject.Get<UnitComponent>().Object.transform.position);
                    followEventEntity.Destroy();
                }
            }
        }

        private void FollowUnitsToTarget()
        {
            var followEventEntities = followEvents.Entities
                .Where(e => e.IsNotNullAndAlive());
            foreach (var followEventEntity in followEventEntities)
            {
                var followEvent = followEventEntity.Get<FollowEvent>();
                var targetComponent = followEvent.Target.Get<UnitComponent>();
                if (targetComponent == null || targetComponent.Object == null)
                {
                    followEventEntity.Destroy();
                    continue;
                }

                UnitAnimationHelper.CreateMovingEvent(ecsWorld, followEvent.MovingObject);
                
                var targetObjectPosition = targetComponent.Object.transform.position;
                UpdateTargetForUnit(followEvent.MovingObject, targetObjectPosition);
            }
        }

        private void CreateAttackEventsIfCanAttackAndNotMoving()
        {
            var allyUnits = units.Entities
                .Where(u => u.IsNotNullAndAlive() && u.Get<UnitComponent>().Tag != UnitTag.EnemyWarrior);
            var enemyUnits = units.Entities
                .Where(u => u.IsNotNullAndAlive() && u.Get<UnitComponent>().Tag == UnitTag.EnemyWarrior);
            
            foreach (var attackingUnitEntity in allyUnits)
            {
                var targetUnitEntity = enemyUnits
                    .FirstOrDefault(enemy => CanAttackAndNotInAttackEvents(attackingUnitEntity, enemy));
                if (targetUnitEntity != default)
                    AttackHelper.CreateAttackEvent(ecsWorld, attackingUnitEntity, targetUnitEntity);
            }
        }

        private bool CanAttackAndNotInAttackEvents(EcsEntity allyUnit, EcsEntity enemyUnit)
        {
            var attackEvent = attackEvents.Entities
                .FirstOrDefault(e => e.IsNotNullAndAlive()
                                     && e.Get<AttackEvent>().AttackingUnit == allyUnit);
            
            return attackEvent == default && AttackHelper.CanAttack(allyUnit, enemyUnit);
        }
    }
}