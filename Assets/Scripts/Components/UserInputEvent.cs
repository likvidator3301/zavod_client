using System.Collections.Generic;
using Systems;
using Entities;
using UnityEngine;

namespace Components
{
    public class UserInputEvent
    {
        private readonly PlayerComponent playerComponent;
        private readonly Dictionary<GameObject, IUnitEntity> units;
        private readonly UnitSystems unitSystems;

        public UserInputEvent(PlayerComponent playerComponent, UnitSystems unitSystems,
            Dictionary<GameObject, IUnitEntity> units)
        {
            this.unitSystems = unitSystems;
            this.playerComponent = playerComponent;
            this.units = units;
        }

        public void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
                if (TryGetHitInfo(out var hitInfo, "EnemyUnit"))
                    foreach (var unit in playerComponent.Units)
                        if (Vector3.Distance(unit.Object.transform.position, hitInfo.point) > unit.Info.AttackRange)
                            unitSystems.MovementSystem.UpdateTargets(hitInfo.point);
                        else
                            unitSystems.AttackSystem.Attack(unit, hitInfo, units);

            if (Input.GetMouseButtonDown(1))
            {
                TryGetHitInfo(out var hitInfo);
                unitSystems.MovementSystem.UpdateTargets(hitInfo.point);
            }

            if (Input.GetKeyDown("u") && playerComponent.Units.Count > 0)
                unitSystems.CreatingSystem.CreateUnit(playerComponent.Units[0].Prefabs, playerComponent, units);
        }

        private bool TryGetHitInfo(out RaycastHit hitInfo, string tagName = "Ground", int range = 1000)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hitInfo, range) && hitInfo.collider.gameObject.CompareTag(tagName);
        }
    }
}