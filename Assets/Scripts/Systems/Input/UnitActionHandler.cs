using Component;
using Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems
{
    public class UnitActionHandler : MonoBehaviour
    {
        private readonly PlayerComponent player;
        private readonly WorldComponent world;
        private readonly RaycastHelper raycastHelper;
        private readonly PrefabsHolderComponent prefabsHolder;

        public UnitActionHandler(
            PlayerComponent player,
            WorldComponent world,
            PrefabsHolderComponent prefabsHolder)
        {
            this.player = player;
            this.world = world;
            this.prefabsHolder = prefabsHolder;
            raycastHelper = new RaycastHelper();
        }

        public void HandleInput()
        {
            HandleCreatingUnits();
            HandleMovingUnits();
        }

        private void HandleCreatingUnits()
        {
            if (Input.GetKeyDown("u"))
            {
                RaycastHelper.TryGetHitInfo(out var hitInfo);
                UnitConditionChangeSystem.CreateUnit(
                    prefabsHolder.WarriorPrefab,
                    UnitTags.Warrior,
                    hitInfo.point,
                    player,
                    raycastHelper,
                    world.Units);
            }
        }

        private void HandleMovingUnits()
        {
            if (Input.GetMouseButtonDown(1))
                MoveSelectedUnits();
        }

        private void MoveSelectedUnits()
        {
            if (!RaycastHelper.TryGetHitInfo(out var hitInfo, UnitTags.EnemyWarrior.ToString()))
                UnitActionSystem.UpdateTargets(hitInfo.point, player.SelectedUnits);
            else
            {
                foreach (var unit in player.SelectedUnits)
                {
                    if (Vector3.Distance(unit.Object.transform.position, hitInfo.point) >
                        unit.StatsComponent.AttackRange)
                        UnitActionSystem.UpdateTargets(hitInfo.point, unit);
                    else
                    {
                        var enemyUnit = world.Units[hitInfo.collider.gameObject];
                        UnitActionSystem.Attack(unit, enemyUnit);
                    }
                }
            }
        }
    }
}