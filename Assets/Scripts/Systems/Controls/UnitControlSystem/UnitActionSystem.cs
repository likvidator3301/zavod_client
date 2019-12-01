﻿using System.Linq;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace Systems
{
    public class UnitActionSystem : IEcsRunSystem
    {
        private const float inertiaEliminatorFactor = 3;
        private EcsWorld ecsWorld;
        private EcsFilter<UnitComponent> units;
        private EcsFilter<AttackEvent> attackEvents;
        private EcsFilter<MoveEvent> moveEvents;
        private EcsFilter<FollowEvent> followEvents;

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

            ecsWorld.NewEntityWith<UnitAnimationEvent>(out var attackAnimationEvent);
            attackAnimationEvent.Unit = attackingUnit;
            attackAnimationEvent.AnimationComponent = new UnitAnimationComponent();
            attackAnimationEvent.AnimationComponent.SetFields(true, false, 1);
            
            targetHealthComponent.CurrentHp -= attackComponent.AttackDamage;
            attackComponent.LastAttackTime = Time.time;
            if (targetHealthComponent.CurrentHp <= 0)
            {
                ecsWorld.NewEntityWith<DeadEvent>(out var deadEvent);
                deadEvent.DeadUnit = targetUnit;
                
                ecsWorld.NewEntityWith<UnitAnimationEvent>(out var dieAnimationEvent);
                dieAnimationEvent.Unit = targetUnit;
                dieAnimationEvent.AnimationComponent = new UnitAnimationComponent();
                dieAnimationEvent.AnimationComponent.SetFields(false, false, 0);
                
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
                ecsWorld.NewEntityWith<UnitAnimationEvent>(out var moveAnimationEvent);
                moveAnimationEvent.Unit = moveEvent.MovingObject;
                moveAnimationEvent.AnimationComponent = new UnitAnimationComponent();
                moveAnimationEvent.AnimationComponent.SetFields(false, true, 1);
                
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

                ecsWorld.NewEntityWith<UnitAnimationEvent>(out var movingEvent);
                movingEvent.Unit = followEvent.MovingObject;
                movingEvent.AnimationComponent = new UnitAnimationComponent();
                movingEvent.AnimationComponent.SetFields(false, true, 1);
                
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
            
            foreach (var unit in allyUnits)
            {
                var unitToAttack = enemyUnits.FirstOrDefault(enemy => CanAttackAndNotInAttackEvents(unit, enemy));
                if (unitToAttack != default)
                {
                    ecsWorld.NewEntityWith<AttackEvent>(out var attackEvent);
                    attackEvent.AttackingUnit = unit;
                    attackEvent.Target = unitToAttack;
                }
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