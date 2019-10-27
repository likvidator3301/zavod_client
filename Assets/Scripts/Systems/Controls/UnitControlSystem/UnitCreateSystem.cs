using System;
using Components;
using Entities;
using UnityEditor;
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
                player.PlayerUnits.Add(Instantiate(prefabs, position, Quaternion.identity));
            }
        }

        public void CreateUnit(GameObject prefabs, Vector3 position, PlayerComponent player)
        {
            player.PlayerUnits.Add(Instantiate(prefabs, position, Quaternion.identity));
        }
    }
}
