using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Systems
{
    public class AttackSystem
    {
        private readonly Dictionary<IUnitEntity, float> nextAttackTime = new Dictionary<IUnitEntity, float>();

        public void Attack(IUnitEntity unit, RaycastHit hitInfo, Dictionary<GameObject, IUnitEntity> units)
        {
            if (units.ContainsKey(hitInfo.collider.gameObject))
            {
                var enemyUnit = units[hitInfo.collider.gameObject];
                if (!nextAttackTime.ContainsKey(unit))
                {
                    nextAttackTime.Add(unit, Time.time + unit.Info.AttackDelay);
                    enemyUnit.Info.CurrentHp -= unit.Info.AttackDamage;
                }
                else if (nextAttackTime[unit] - Time.time < 0)
                {
                    nextAttackTime[unit] = Time.time + unit.Info.AttackDelay;
                    enemyUnit.Info.CurrentHp -= unit.Info.AttackDamage;
                }

                if (unit.Info.CurrentHp <= 0)
                {
                    nextAttackTime.Remove(unit);
                }
            }
        }
    }
}