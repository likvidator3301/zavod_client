using UnityEditor;
using UnityEngine;

namespace Components
{
    public interface IUnitInfo
    {
        GUID PlayerGuid { get; }
        float AttackDamage { get; }
        float AttackSpeed { get; }
        float AttackRange { get; }
        float Defense { get; }
        float MoveSpeed { get; }
        float MaxHp { get; }
        float CurrentHp { get; set; }
        Vector3 Coords { get; set; }
    }
}