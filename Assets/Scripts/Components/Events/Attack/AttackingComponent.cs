using Leopotam.Ecs;
using UnityEngine;

namespace Components.Attack
{
    public class AttackingComponent
    {
        public EcsEntity TargetEntity { get; set; }
        public HealthComponent TargetHealthComponent { get; set; }
        public MovementComponent TargetMovementComponent { get; set; }
    }
}