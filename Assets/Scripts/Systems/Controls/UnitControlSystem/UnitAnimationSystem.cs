using System.Linq;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UnitAnimationSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitAnimationEvent> animationEvents = null;
        private readonly EcsFilter<UnitComponent> units = null;
        private const float stopMovingAnimationDistance = 1.5f;

        public void Run()
        {
            RunAnimations();
            StopUnitIfIsOnTargetPosition();
            StopAttackingIfNotAttacking();
        }

        private void RunAnimations()
        {
            var animationEventEntities = animationEvents.Entities.Where(a => a.IsNotNullAndAlive());
            foreach (var animationEventEntity in animationEventEntities)
            {
                var unitComponent = animationEventEntity.Get<UnitComponent>();
                var animationComponent = animationEventEntity.Get<UnitAnimationEvent>().AnimationComponent;
                if (unitComponent != null)
                {
                    var animator = unitComponent.Animator;
                    UnitAnimationHelper.SetFieldForAnimatorFromComponent(animator, animationComponent);
                }
                
                animationEventEntity.Unset<AnimationEvent>();
            }
        }

        private void StopUnitIfIsOnTargetPosition()
        {
            var unitEntities = units.Entities
                .Where(u => u.IsNotNullAndAlive());
            foreach (var unitEntity in unitEntities)
            {
                var unitComponent = unitEntity.Get<UnitComponent>();
                var currentPosition = unitComponent.Object.transform.position;
                if (Vector3.Distance(unitComponent.Agent.pathEndPosition, currentPosition) <= stopMovingAnimationDistance)
                    UnitAnimationHelper.CreateStopMovingEvent(unitEntity);
            }
        }
        
        private void StopAttackingIfNotAttacking()
        {
            var unitEntities = units.Entities
                .Where(u => u.IsNotNullAndAlive());
            foreach (var unitEntity in unitEntities)
            {
                var isAttacking = unitEntity.Get<AttackEvent>() != null;
                
                if (!isAttacking)
                    UnitAnimationHelper.CreateStopAttackingEvent(unitEntity);
            }
        }
    }
}