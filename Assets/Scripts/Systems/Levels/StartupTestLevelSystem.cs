using System;
using System.Collections.Generic;
using Components;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Systems
{
    public class StartupTestLevelSystem : MonoBehaviour
    {
        private UserInputEvent userInputEvent;
        private PlayerComponent playerComponent;
        private UnitSystems unitSystems;

        void Start()
        {
            unitSystems = new UnitSystems();
            var firstPlayerGuid = GUID.Generate();
            //var secondPlayerGuid = GUID.Generate();
            playerComponent = new PlayerComponent(firstPlayerGuid);
            userInputEvent = new UserInputEvent(playerComponent, new UnitSystems());

            var firstUnit = new WarriorEntity(new Vector3(400, 2.5f, 500));
            var secondUnit = new WarriorEntity(new Vector3(400, 2.5f, 525));
            var firstObjectUnit = Instantiate(firstUnit.Prefabs, firstUnit.UnitInfo.Coords, Quaternion.identity);
            var secondObjectUnit = Instantiate(secondUnit.Prefabs, secondUnit.UnitInfo.Coords, Quaternion.identity);

            firstObjectUnit.gameObject.tag = "Unit";
            firstObjectUnit.GetComponent<MeshRenderer>().material.color = Color.white;
            firstUnit.Object = firstObjectUnit;
            secondObjectUnit.gameObject.tag = "EnemyUnit";
            secondObjectUnit.GetComponent<MeshRenderer>().material.color = Color.black;
            secondUnit.Object = secondObjectUnit;

            playerComponent.Units.Add(firstUnit);
        }

        void Update()
        {
            userInputEvent.HandleInput();
        }
    }
}
