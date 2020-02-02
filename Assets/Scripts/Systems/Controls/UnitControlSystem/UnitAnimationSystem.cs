using System.Linq;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UnitAnimationSystem : IEcsRunSystem
    {
        private ServerIntegration.ServerIntegration serverIntegration;
        private readonly EcsFilter<UnitAnimationEvent> animationEvents = null;
        private readonly EcsFilter<UnitComponent> units = null;
        private const float stopMovingAnimationDistance = 1.5f;

        public void Run()
        {
            RunAnimations();
            StopUnitIfOnTargetPosition();
            StopAttackIfNotAttacking();
            
            AddMoveUnitsToClient();
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

        private void StopUnitIfOnTargetPosition()
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
        
        private void StopAttackIfNotAttacking()
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
        
        //TODO: Try to explain why u.Get<UnitAnimationComponent> can be null!
        private void AddMoveUnitsToClient()
        {
            var movingUnitsEntities = units.Entities
                .Where(u => u.IsNotNullAndAlive() && u.Get<UnitAnimationComponent>() != null
                            && u.Get<UnitAnimationComponent>().IsMoving);
            foreach (var movingUnit in movingUnitsEntities)
            {
                var unitComponent = movingUnit.Get<UnitComponent>();
                var unityPosition = unitComponent.Object.transform.position;
                var position = new Models.Vector3 {X = unityPosition.x, Y = unityPosition.y, Z = unityPosition.z};
                serverIntegration.client.Unit.AddUnitsToMove(unitComponent.Guid, position);
            }
        }
    }
}