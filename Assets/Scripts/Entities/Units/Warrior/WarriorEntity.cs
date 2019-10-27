using Components;
using UnityEditor;
using UnityEngine;

namespace Entities
{
    public class WarriorEntity : IUnitEntity
    {
        public IUnitInfo UnitInfo { get; }
        public GameObject Prefabs { get; }
        public GameObject Object { get; set; }

        public WarriorEntity(Vector3 position)
        {
            this.UnitInfo = new WarriorComponent(position);
            Prefabs = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Textures/Prefabs/Units/Warrior/WarriorPrefabs.prefab",
                typeof(GameObject));
        }
    }
}
