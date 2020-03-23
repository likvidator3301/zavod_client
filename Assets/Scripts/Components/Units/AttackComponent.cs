﻿using Models;
using UnityEngine;

namespace Components
{
    public class AttackComponent
    {
        public float AttackDamage { get; set; }
        public float AttackDelay { get; set; }
        public float AttackRange { get; set; }
        public float LastAttackTime { get; set; }

        public void InitializeComponent(ServerUnitDto unitDto)
        {
            AttackDamage = unitDto.AttackDamage;
            AttackDelay = unitDto.AttackDelay;
            AttackRange = unitDto.AttackRange;
            LastAttackTime = unitDto.LastAttackTime;
        }
    }
}