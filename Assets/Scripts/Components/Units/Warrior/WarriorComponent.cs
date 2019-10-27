using UnityEditor;
using UnityEngine;

namespace Components
{
    public class WarriorComponent : IUnitInfo
    {
        public GUID PlayerGuid { get; }
        public float AttackDamage => 10;
        public float AttackSpeed => 15;
        public float AttackRange => 10;
        public float Defense => 10;
        public float MoveSpeed => 10;
        public float MaxHp => 50;
        public float CurrentHp { get; set; }
        public Vector3 Coords { get; set; }

        public WarriorComponent(GUID playerGuid, Vector3 coords)
        {
            this.PlayerGuid = playerGuid;
            this.Coords = coords;
        }
    }
}
