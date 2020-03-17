using System.Diagnostics;
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
        private const float stopMovingAnimationDistance = 0.1f;
        private const int sendMovingUnitsDelay = 100;
        private Stopwatch lastSendMovingUnitsTime = new Stopwatch();

        public void Run()
        {
            RunAnimations();
            StopAttackingUnitsWithoutTargets();
            StopUnitsOnTargetPosition();
            
            AddMovingUnitsToClient();
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

        private void StopUnitsOnTargetPosition()
        {
            var movingUnits = units.Entities
                .Where(u => u.IsNotNullAndAlive());
            foreach (var unitEntity in movingUnits)
            {
                var unitComponent = unitEntity.Get<UnitComponent>();
                var currentPosition = unitComponent.Object.transform.position;
                if (Vector3.Distance(unitComponent.Agent.pathEndPosition, currentPosition) <= stopMovingAnimationDistance)
                    UnitAnimationHelper.CreateStopMovingEvent(unitEntity);
            }
        }
        
        private void StopAttackingUnitsWithoutTargets()
        {
            var unitEntities = units.Entities
                .Where(u => u.IsNotNullAndAlive()
                        && (u.Get<AttackEvent>() == null 
                        || u.Get<AttackEvent>().TargetHealthComponent == null 
                        || u.Get<AttackEvent>().TargetHealthComponent.CurrentHp <= 0));
            foreach (var unitEntity in unitEntities)
            {
                if (unitEntity.Get<UnitAnimationComponent>() == null || unitEntity.Get<UnitAnimationComponent>().IsAttacking)
                    UnitAnimationHelper.CreateStopAttackingEvent(unitEntity);
            }
        }
        
        private void AddMovingUnitsToClient()
        {
            if (lastSendMovingUnitsTime.ElapsedMilliseconds <= sendMovingUnitsDelay)
                return;
            lastSendMovingUnitsTime.Restart();
            
            var movingUnitsEntities = units.Entities
                .Where(u => u.IsNotNullAndAlive()
                            && u.Get<UnitAnimationComponent>() != null
                            && u.Get<UnitAnimationComponent>().IsMoving);
            foreach (var movingUnit in movingUnitsEntities)
            {
                var unitComponent = movingUnit.Get<UnitComponent>();
                var unityPosition = unitComponent.Object.transform.position;
                var position = new Models.Vector3 {X = unityPosition.x, Y = unityPosition.y, Z = unityPosition.z};
                ServerCommunication.ServerClient.Client.Unit.AddUnitsToMove(unitComponent.Guid, position);
            }
        }
    }
}