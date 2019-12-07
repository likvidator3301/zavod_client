using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UnitAnimationHelper
    {
        public static void CreateMovingEvent(EcsEntity unitEntity)
        {
            var unitHealth = unitEntity.Get<HealthComponent>().CurrentHp;
            var movingAnimationEvent = GetEventWithUnit(unitEntity);
            movingAnimationEvent.AnimationComponent.SetFields(false, true, unitHealth);
        }

        public static void CreateAttackEvent(EcsEntity unitEntity)
        {
            var unitHealth = unitEntity.Get<HealthComponent>().CurrentHp;
            var attackAnimationEvent = GetEventWithUnit(unitEntity);
            attackAnimationEvent.AnimationComponent.SetFields(true, false, unitHealth);
        }

        public static void CreateDieEvent(EcsEntity unitEntity)
        {
            var dieAnimationEvent = GetEventWithUnit(unitEntity);
            dieAnimationEvent.AnimationComponent.SetFields(false, false, 0);
        }

        public static void CreateStopMovingEvent(EcsEntity unitEntity)
        {
            var previousAttackingState = unitEntity.Get<UnitComponent>().Animator.GetBool(
                UnitAnimationState.IsAttacking.ToString());
            var unitHealth = unitEntity.Get<UnitComponent>().Animator.GetFloat(
                UnitAnimationState.CurrentHp.ToString());
            var animationEvent = GetEventWithUnit(unitEntity);
            animationEvent.AnimationComponent.SetFields(previousAttackingState, false, unitHealth);
        }

        public static void CreateStopAttackingEvent(EcsEntity unitEntity)
        {
            var unitHealth = unitEntity.Get<UnitComponent>().Animator.GetFloat(
                UnitAnimationState.CurrentHp.ToString());
            var attackCancelEvent = GetEventWithUnit(unitEntity);
            attackCancelEvent.AnimationComponent.SetFields(false, false, unitHealth);
        }
        
        public static void SetFieldForAnimatorFromComponent(Animator animator, UnitAnimationComponent animationComponent)
        {
            animator.SetBool(UnitAnimationState.IsAttacking.ToString(), animationComponent.IsAttacking);
            animator.SetBool(UnitAnimationState.IsMoving.ToString(), animationComponent.IsMoving);
            animator.SetFloat(UnitAnimationState.CurrentHp.ToString(), animationComponent.CurrentHp);
        }

        private static UnitAnimationEvent GetEventWithUnit(EcsEntity unitEntity)
        {
            var newEvent = unitEntity.Set<UnitAnimationEvent>();
            newEvent.AnimationComponent = new UnitAnimationComponent();
            return newEvent;
        }
    }
}