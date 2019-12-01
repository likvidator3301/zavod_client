using System.Linq;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UnitAnimationSystem : IEcsRunSystem
    {
        private EcsWorld ecsWorld;
        private EcsFilter<UnitAnimationEvent> animationEvents;
        private EcsFilter<UnitComponent> units;
        private EcsFilter<AttackEvent> attackEvents;
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
                    SetFieldForAnimatorFromComponent(animator, animationComponent);
                }
                
                animationEventEntity.Destroy();
            }
        }

        private void SetFieldForAnimatorFromComponent(Animator animator, UnitAnimationComponent animationComponent)
        {
            animator.SetBool(UnitAnimationState.IsAttacking.ToString(), animationComponent.IsAttacking);
            animator.SetBool(UnitAnimationState.IsMoving.ToString(), animationComponent.IsMoving);
            animator.SetFloat(UnitAnimationState.CurrentHp.ToString(), animationComponent.CurrentHp);
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