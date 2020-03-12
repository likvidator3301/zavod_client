using Leopotam.Ecs;
using UnityEngine;

namespace Components.Attack
{
    public class StartAttackingEvent
    {
        public EcsEntity TargetEntity { get; set; }
        public HealthComponent TargetHealthComponent { get; set; }
        public MovementComponent TargetMovementComponent { get; set; }
    }
}