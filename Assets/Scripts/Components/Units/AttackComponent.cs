﻿using UnityEngine;

namespace Components
{
    public class AttackComponent : MonoBehaviour
    {
        public float AttackDamage { get; set; }
        public float AttackDelay { get; set; }
        public float AttackRange { get; set; }
        public float LastAttackTime { get; set; }

        public void InitializeComponent(AttackComponent attackComponent)
        {
            AttackDamage = attackComponent.AttackDamage;
            AttackDelay = attackComponent.AttackDelay;
            AttackRange = attackComponent.AttackRange;
            LastAttackTime = attackComponent.LastAttackTime;
        }
    }
}