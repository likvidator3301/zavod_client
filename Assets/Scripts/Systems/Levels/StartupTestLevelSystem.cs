using System.Collections.Generic;
using Components;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Systems
{
    public class StartupTestLevelSystem : MonoBehaviour
    {
        private readonly Vector3 firstUnitPlace = new Vector3(400, 2.6f, 500);
        private readonly Vector3 secondUnitPlace = new Vector3(400, 2.6f, 525);
        private readonly Dictionary<GameObject, IUnitEntity> units = new Dictionary<GameObject, IUnitEntity>();
        private readonly Color friendlyUnitColor = Color.cyan;
        private readonly Color enemyUnitColor = Color.black;
        private PlayerComponent playerComponent;
        private UnitSystems unitSystems;
        private UserInputEvent userInput;

        private void Start()
        {
            var firstPlayerGuid = GUID.Generate();
            playerComponent = new PlayerComponent(firstPlayerGuid);

            var firstUnit = new WarriorEntity();
            var secondUnit = new WarriorEntity();
            var firstObjectUnit = Instantiate(firstUnit.Prefabs, firstUnitPlace, Quaternion.identity);
            var secondObjectUnit = Instantiate(secondUnit.Prefabs, secondUnitPlace, Quaternion.identity);

            firstObjectUnit.gameObject.tag = "Unit";
            firstObjectUnit.GetComponent<MeshRenderer>().material.color = friendlyUnitColor;
            firstUnit.Object = firstObjectUnit;
            secondObjectUnit.gameObject.tag = "EnemyUnit";
            secondObjectUnit.GetComponent<MeshRenderer>().material.color = enemyUnitColor;
            secondUnit.Object = secondObjectUnit;

            units.Add(firstUnit.Object, firstUnit);
            units.Add(secondUnit.Object, secondUnit);
            playerComponent.Units.Add(firstUnit);

            unitSystems = new UnitSystems(playerComponent, units);
            userInput = new UserInputEvent(playerComponent, unitSystems, units);
        }

        private void Update()
        {
            userInput.HandleInput();
            unitSystems.DestroyingSystem.HandleDestroy(playerComponent);
        }
    }
}