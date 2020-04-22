using Models;
using System;
using UnityEngine;

namespace Components
{
    public class AttackComponent
    {
        public float AttackDamage { get; set; }
        public TimeSpan AttackDelay { get; set; }
        public float AttackRange { get; set; }
        public DateTime LastAttackTime { get; set; }

        public void InitializeComponent(UnitType unitType)
        {
            if (unitType == UnitType.Warrior)
            {
                AttackDamage = 10;
                AttackDelay = TimeSpan.FromMilliseconds(800);
                AttackRange = 1;
            }

            if (unitType == UnitType.Runner)
            {
                AttackDamage = 0;
                AttackDelay = TimeSpan.FromMilliseconds(1488000); ;
                AttackRange = 0;
            }
        }
    }
}