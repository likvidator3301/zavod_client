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
                if (TryGetHitInfo(out var hitInfo, "EnemyUnit"))
                {
                    foreach (var unit in playerComponent.Units)
                    {
                        if (Vector3.Distance(unit.UnitInfo.Coords, hitInfo.point) < unit.UnitInfo.AttackRange)
                            unit.UnitInfo.NextCoords = hitInfo.point;
                        else
                        {
                            //unitSystems.AttackSystem.Attack();
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {

            }

            if (Input.GetKeyDown("u") && playerComponent.Units.Count > 0)
            {
                unitSystems.CreatingSystem.CreateUnit(playerComponent.Units[0].Prefabs, playerComponent);
            }
        }

        private bool TryGetHitInfo(out RaycastHit hitInfo, string tagName = "", int range = 100)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hitInfo, range) && hitInfo.collider.gameObject.CompareTag(tagName);
        }
    }
}
