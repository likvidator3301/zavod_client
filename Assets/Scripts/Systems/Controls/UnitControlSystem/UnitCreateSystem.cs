using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Entities;
using UnityEngine;

namespace Systems
{
    public class CreatingSystem : MonoBehaviour
    {
        private const float minHeight = 2.5f;

        public void CreateUnit(GameObject prefabs, PlayerComponent player, Dictionary<GameObject, IUnitEntity> units)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 100))
            {
                var position = hitInfo.point;
                position.y = Math.Min(position.y, minHeight);
                CreateUnit(prefabs, position, player, units);
            }
        }

        public void CreateUnit(
            GameObject prefabs,
            Vector3 position,
            PlayerComponent player,
            Dictionary<GameObject, IUnitEntity> units)
        {
            var newUnit = Instantiate(prefabs, position, Quaternion.identity);
            newUnit.gameObject.tag = prefabs.tag;
            player.Units.Add(new WarriorEntity());
            player.Units.Last().Object = newUnit;
            units.Add(newUnit, player.Units.Last());
        }
    }
}