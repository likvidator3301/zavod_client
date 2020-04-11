using Models;
using UnityEngine;

namespace Components
{
    public class AttackComponent
    {
        public float AttackDamage { get; set; }
        public float AttackDelay { get; set; }
        public float AttackRange { get; set; }
        public float LastAttackTime { get; set; }

        public void InitializeComponent(UnitType unitType)
        {
        }
    }
}