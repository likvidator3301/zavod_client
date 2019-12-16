using Leopotam.Ecs;
using UnityEngine;

namespace Components.UnitsEvents
{
    public class MoveEvent
    {
        public EcsEntity MovingObject { get; set; }
        public Vector3 NextPosition { get; set; }
    }
}