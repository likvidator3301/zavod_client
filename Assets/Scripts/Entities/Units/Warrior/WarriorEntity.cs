using Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Entities
{
    public class WarriorEntity : IUnitEntity
    {
        public IUnitInfo Info { get; }
        public GameObject Prefabs { get; }
        public GameObject Object { get; set; }
        public NavMeshAgent Agent => Object != null ? Object.gameObject.GetComponent<NavMeshAgent>() : null;

        public WarriorEntity()
        {
            this.Info = new WarriorComponent();
            Prefabs = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Textures/Prefabs/Units/Warrior/WarriorPrefabs.prefab",
                typeof(GameObject));
        }
    }
}
