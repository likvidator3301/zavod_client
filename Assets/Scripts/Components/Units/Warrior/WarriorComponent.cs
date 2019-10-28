using UnityEngine;

namespace Components
{
    public class WarriorComponent : IUnitInfo
    {
        public float AttackDamage => 10;
        public float AttackSpeed => 15;
        public float AttackRange => 10;
        public float Defense => 10;
        public float MoveSpeed => 25;
        public float MaxHp => 50;
        public float CurrentHp { get; set; }
        public Vector3 Coords { get; set; }
        public Vector3 NextCoords { get; set; }
    }
}
