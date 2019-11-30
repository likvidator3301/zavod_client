using System.Linq;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

namespace Systems
{
    public class UnitAnimationSystem : IEcsRunSystem
    {
        private EcsWorld ecsWorld;
        private EcsFilter<UnitAnimationEvent> animationEvents;
        private EcsFilter<UnitComponent> units;
        private EcsFilter<AttackEvent> attackEvents;

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
                if (Vector3.Distance(unit.Agent.destination, currentPosition) <= 1.5)
                {
                    var previousAttackingState = unit.Animator.GetBool(UnitAnimationState.IsAttacking.ToString());
                    var previousHpState = unit.Animator.GetFloat(UnitAnimationState.CurrentHp.ToString());
                    ecsWorld.NewEntityWith<UnitAnimationEvent>(out var animationEvent);
                    animationEvent.Unit = unitEntity;
                    animationEvent.AnimationComponent = new UnitAnimationComponent();
                    animationEvent.AnimationComponent.SetFields(previousAttackingState, false, previousHpState);
                }
            }
        }
        
        private void ChangeAttackStateIfNotAttacking()
        {
            var unitEntities = units.Entities.Where(u => u.IsNotNullAndAlive());
            foreach (var unitEntity in unitEntities)
            {
                var unit = unitEntity.Get<UnitComponent>();
                var isAttacking = attackEvents.Entities
                                      .FirstOrDefault(attackEntity => 
                                          attackEntity.IsNotNullAndAlive()
                                          && attackEntity.Get<AttackEvent>().AttackingUnit == unitEntity) != default;
                if (!isAttacking)
                {
                    ecsWorld.NewEntityWith<UnitAnimationEvent>(out var attackCancelEvent);
                    var previousMovingState = unit.Animator.GetBool(UnitAnimationState.IsMoving.ToString());
                    var previousHpState = unit.Animator.GetFloat(UnitAnimationState.CurrentHp.ToString());
                    attackCancelEvent.Unit = unitEntity;
                    attackCancelEvent.AnimationComponent = new UnitAnimationComponent();
                    attackCancelEvent.AnimationComponent.SetFields(
                        false,
                        previousMovingState,
                        previousHpState);
                }
            }
        }
    }
}