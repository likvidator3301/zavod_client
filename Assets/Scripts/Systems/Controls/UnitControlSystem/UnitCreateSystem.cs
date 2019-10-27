using System;
using System.Linq;
using Components;
using Entities;
using UnityEngine;

namespace Systems
{
    public class CreatingSystem : MonoBehaviour
    {
        private const float minHeight = 2.5f;

        public void CreateUnit(GameObject prefabs, PlayerComponent player)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 100) && hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                var position = hitInfo.point;
                position.y = Math.Min(position.y, minHeight);
                var newUnit = Instantiate(prefabs, position, Quaternion.identity);
                newUnit.gameObject.tag = "Unit";
                player.Units.Add(new WarriorEntity(newUnit.transform.position));
                player.Units.Last().Object = newUnit;
            }
        }

        public void CreateUnit(GameObject prefabs, Vector3 position, PlayerComponent player)
        {
            var newUnit = Instantiate(prefabs, position, Quaternion.identity);
            newUnit.gameObject.tag = "Unit";
            player.Units.Add(new WarriorEntity(newUnit.transform.position));
            player.Units.Last().Object = newUnit;
        }
    }
}
