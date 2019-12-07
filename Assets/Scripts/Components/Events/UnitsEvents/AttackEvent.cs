using UnityEngine;

namespace Components.UnitsEvents
{
    public class AttackEvent
    {
        public Vector3 TargetPosition { get; set; }
        public HealthComponent TargetHealthComponent { get; set; }
    }
}