using System;
using System.Collections.Generic;
using Components;
using Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Systems
{
    public class StartupTestLevelSystem : MonoBehaviour
    {
        private UserInputEvent userInputEvent;
        private PlayerComponent playerComponent;
        private UnitSystems unitSystems;
        private readonly Vector3 firstUnitPlace = new Vector3(400, 2.6f, 500);
        private readonly Vector3 secondUnitPlace = new Vector3(400, 2.6f, 525);

        void Start()
        {
            var firstPlayerGuid = GUID.Generate();
            //var secondPlayerGuid = GUID.Generate();
            playerComponent = new PlayerComponent(firstPlayerGuid);

            var firstUnit = new WarriorEntity();
            var secondUnit = new WarriorEntity();
            var firstObjectUnit = Instantiate(firstUnit.Prefabs, firstUnitPlace, Quaternion.identity);
            var secondObjectUnit = Instantiate(secondUnit.Prefabs, secondUnitPlace, Quaternion.identity);

            firstObjectUnit.gameObject.tag = "Unit";
            firstObjectUnit.GetComponent<MeshRenderer>().material.color = Color.cyan;
            firstUnit.Object = firstObjectUnit;
            secondObjectUnit.gameObject.tag = "EnemyUnit";
            secondObjectUnit.GetComponent<MeshRenderer>().material.color = Color.black;
            secondUnit.Object = secondObjectUnit;

            playerComponent.Units.Add(firstUnit);

            unitSystems = new UnitSystems(playerComponent.Units);
            userInputEvent = new UserInputEvent(playerComponent, unitSystems);
        }

        void Update()
        {
            userInputEvent.HandleInput();
        }
    }
}
