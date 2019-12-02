using System.Linq;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UnitAnimationSystem : IEcsRunSystem
    {
        private readonly EcsWorld ecsWorld = null;
        private readonly EcsFilter<UnitAnimationEvent> animationEvents = null;
        private readonly EcsFilter<UnitComponent> units = null;
        private readonly EcsFilter<AttackEvent> attackEvents = null;
        private const float StopMovingAnimationDistance = 1.5f;

        public void Run()
        {
            RunAnimations();
            ChangeMovingStateIfIsOnTargetPosition();
            ChangeAttackStateIfNotAttacking();
        }

        private void RunAnimations()
        {
            var animationEventEntities = animationEvents.Entities.Where(a => a.IsNotNullAndAlive());
            foreach (var animationEventEntity in animationEventEntities)
            {
                var unit = animationEventEntity.Get<UnitAnimationEvent>().Unit;
                var animationComponent = animationEventEntity.Get<UnitAnimationEvent>().AnimationComponent;
                if (unit.IsNotNullAndAlive())
                {
                    var animator = unit.Get<UnitComponent>().Animator;
                    UnitAnimationHelper.SetFieldForAnimatorFromComponent(animator, animationComponent);
                }
                
                animationEventEntity.Destroy();
            }
        }

        private void ChangeMovingStateIfIsOnTargetPosition()
        {
            var unitEntities = units.Entities.Where(u => u.IsNotNullAndAlive());
            foreach (var unitEntity in unitEntities)
            {
                var unit = unitEntity.Get<UnitComponent>();
                var currentPosition = unit.Object.transform.position;
                if (Vector3.Distance(unit.Agent.destination, currentPosition) <= StopMovingAnimationDistance)
                    UnitAnimationHelper.CreateStopMovingEvent(ecsWorld, unitEntity);
            }
        }
        
        private void ChangeAttackStateIfNotAttacking()
        {
            var unitEntities = units.Entities.Where(u => u.IsNotNullAndAlive());
            foreach (var unitEntity in unitEntities)
            {
                var isAttacking = attackEvents.Entities
                                      .FirstOrDefault(attackEntity => 
                                          attackEntity.IsNotNullAndAlive()
                                          && attackEntity.Get<AttackEvent>().AttackingUnit == unitEntity) != default;
                if (!isAttacking)
                    UnitAnimationHelper.CreateStopAttackingEvent(ecsWorld, unitEntity);
            }
        }
    }
}