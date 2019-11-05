using System;
using System.Collections.Generic;
using System.Linq;
using Component;
using Components;
using Entities;
using UnityEngine;

namespace Systems
{
    public class UnitConditionChangeSystem : MonoBehaviour
    {
        private const float minHeight = 2.5f;

        public void CreateUnit(
            GameObject prefab,
            UnitTags unitTag,
            Vector3 position,
            PlayerComponent player,
            RaycastHelper raycastHelper,
            Dictionary<GameObject, IUnitEntity> units)
        {
            position.y = Math.Min(position.y, minHeight);
            var newUnit = Instantiate(prefab, position, Quaternion.identity);
            switch (unitTag)
            {
                case UnitTags.Warrior:
                    {
                        var warriorEntity = new WarriorEntity(newUnit, UnitTags.Warrior);
                        units.Add(newUnit, warriorEntity);
                        break;
                    }
                case UnitTags.EnemyWarrior:
                    {
                        var warriorEntity = new WarriorEntity(newUnit, UnitTags.EnemyWarrior);
                        units.Add(newUnit, warriorEntity);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void DestroyDeadUnits(PlayerComponent player, Dictionary<GameObject, IUnitEntity> units)
        {
            var toDelete = new List<IUnitEntity>();
            foreach (var unit in units.Values)
            {
                if (unit.ConditionComponent.CurrentHp <= 0 && units.ContainsKey(unit.Object))
                    toDelete.Add(unit);
            }

            foreach (var unit in toDelete)
            {
                if (player.HighlightedUnits.Contains(unit))
                    player.HighlightedUnits.Remove(unit);
                units.Remove(unit.Object);
                Destroy(unit.Object);
            }
        }
    }
}