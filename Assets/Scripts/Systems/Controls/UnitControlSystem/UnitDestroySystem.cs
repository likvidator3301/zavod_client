using System.Collections.Generic;
using Components;
using Entities;
using UnityEngine;

namespace Systems
{
    public class DestroyingSystem : MonoBehaviour
    {
        public void HandleDestroy(Dictionary<GameObject, IUnitEntity> units, PlayerComponent player)
        {
            var toRemoveList = new List<(GameObject, IUnitEntity)>();
            foreach (var unitObj in units.Keys)
            {
                var unit = units[unitObj];
                Debug.Log(unit.Info.CurrentHp);
                if (unit.Info.CurrentHp <= 0)
                    toRemoveList.Add((unitObj, unit));
            }

            foreach (var (unitObj, unit) in toRemoveList)
            {
                units.Remove(unitObj);
                player.Units.Remove(unit);
                Destroy(unitObj);
            }
        }
    }
}