using System;
using UnityEngine;
using Systems;

namespace Components
{
    public class UserInputEvent
    {
        private readonly UnitSystems unitSystems;
        private readonly PlayerComponent playerComponent;

        public UserInputEvent(PlayerComponent playerComponent, UnitSystems unitSystems)
        {
            this.unitSystems = unitSystems;
            this.playerComponent = playerComponent;
        }

        public void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (TryGetHitInfo(out var hitInfo, "Unit"))
                {
                    //if (!playerComponent.Guid != hitInfo.collider.gameObject.GetComponent())
                }
            }

            if (Input.GetMouseButtonDown(1))
            {

            }

            if (Input.GetKeyDown("u") && playerComponent.PlayerUnits.Count > 0)
            {
                unitSystems.CreatingSystem.CreateUnit(playerComponent.PlayerUnits[0], playerComponent);
            }
        }

        private bool TryGetHitInfo(out RaycastHit hitInfo, string tagName = "", int range = 100)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hitInfo, range) && hitInfo.collider.gameObject.CompareTag(tagName);
        }
    }
}
