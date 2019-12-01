using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UnitAnimationHelper
    {
        public static void CreateMovingEvent(EcsWorld ecsWorld, EcsEntity unitEntity)
        {
            var unitHealth = unitEntity.Get<HealthComponent>().CurrentHp;
            var movingAnimationEvent = GetEventWithUnit(ecsWorld, unitEntity);
            movingAnimationEvent.AnimationComponent.SetFields(false, true, unitHealth);
        }

        public static void CreateAttackEvent(EcsWorld ecsWorld, EcsEntity unitEntity)
        {
            var unitHealth = unitEntity.Get<HealthComponent>().CurrentHp;
            var attackAnimationEvent = GetEventWithUnit(ecsWorld, unitEntity);
            attackAnimationEvent.AnimationComponent.SetFields(true, false, unitHealth);
        }

        public static void CreateDieEvent(EcsWorld ecsWorld, EcsEntity unitEntity)
        {
            var dieAnimationEvent = GetEventWithUnit(ecsWorld, unitEntity);
            dieAnimationEvent.AnimationComponent.SetFields(false, false, 0);
        }

        public static void CreateStopMovingEvent(EcsWorld ecsWorld, EcsEntity unitEntity)
        {
            var previousAttackingState = unitEntity.Get<UnitComponent>().Animator.GetBool(
                UnitAnimationState.IsAttacking.ToString());
            var previousHpState = unitEntity.Get<UnitComponent>().Animator.GetFloat(
                UnitAnimationState.CurrentHp.ToString());
            var animationEvent = GetEventWithUnit(ecsWorld, unitEntity);
            animationEvent.AnimationComponent.SetFields(previousAttackingState, false, previousHpState);
        }

        public static void CreateStopAttackingEvent(EcsWorld ecsWorld, EcsEntity unitEntity)
        {            
            var previousMovingState = unitEntity.Get<UnitComponent>().Animator.GetBool(
                UnitAnimationState.IsMoving.ToString());
            var previousHpState = unitEntity.Get<UnitComponent>().Animator.GetFloat(
                UnitAnimationState.CurrentHp.ToString());
            var attackCancelEvent = GetEventWithUnit(ecsWorld, unitEntity);
            attackCancelEvent.AnimationComponent.SetFields(false, previousMovingState, previousHpState);
        }
        
        public static void SetFieldForAnimatorFromComponent(Animator animator, UnitAnimationComponent animationComponent)
        {
            animator.SetBool(UnitAnimationState.IsAttacking.ToString(), animationComponent.IsAttacking);
            animator.SetBool(UnitAnimationState.IsMoving.ToString(), animationComponent.IsMoving);
            animator.SetFloat(UnitAnimationState.CurrentHp.ToString(), animationComponent.CurrentHp);
        }

        private static UnitAnimationEvent GetEventWithUnit(EcsWorld ecsWorld, EcsEntity unitEntity)
        {
            ecsWorld.NewEntityWith<UnitAnimationEvent>(out var newEvent);
            newEvent.Unit = unitEntity;
            newEvent.AnimationComponent = new UnitAnimationComponent();
            return newEvent;
        }
    }
}