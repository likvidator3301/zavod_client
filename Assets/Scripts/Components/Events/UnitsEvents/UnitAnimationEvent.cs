using Leopotam.Ecs;
using UnityEngine;

namespace Components.UnitsEvents
{
    public class UnitAnimationEvent
    {
        public EcsEntity Unit { get; set; }
        public UnitAnimationComponent AnimationComponent { get; set; }
    }
}