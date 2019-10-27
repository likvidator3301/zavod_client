using System.Collections.Generic;
using Components;
using UnityEditor;
using UnityEngine;

namespace Systems
{
    public class StartupTestLevelSystem : MonoBehaviour
    {
        public GameObject WarriorPrefabs;
        private UserInputEvent userInputEvent = new UserInputEvent();
        private readonly List<IUnitInfo> unitsInfo = new List<IUnitInfo>();

        void Start()
        {
            var firstPlayerGuid = GUID.Generate();
            var secondPlayerGuid = GUID.Generate();
            unitsInfo.Add(new WarriorComponent(firstPlayerGuid, new Vector3(400, 2.5f, 500)));
            unitsInfo.Add(new WarriorComponent(secondPlayerGuid, new Vector3(400, 2.5f, 525)));
            var firstUnit = Instantiate(WarriorPrefabs, unitsInfo[0].Coords, Quaternion.identity);
            var secondUnit = Instantiate(WarriorPrefabs, unitsInfo[1].Coords, Quaternion.identity);

            firstUnit.GetComponent<MeshRenderer>().material.color = Color.white;
            secondUnit.GetComponent<MeshRenderer>().material.color = Color.black;
        }
    }
}
