using UnityEngine;

namespace Components
{
    public class WarriorConditionComponent : IConditionComponent
    {
        public float MaxHp => 50;
        public float CurrentHp { get; set; } = 50;
        public float LastAttackTime { get; set; } = Time.time;
    }
}
