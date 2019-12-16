using System.IO;
using UnityEngine;

namespace Components
{
    public static class UnitsPrefabsHolder
    {
        public static readonly GameObject WarriorPrefab;
        public static readonly GameObject EnemyWarriorPrefab;
        private const string pathToPrefabs = @"Prefabs/Units";

        static UnitsPrefabsHolder()
        {
            WarriorPrefab =  Resources.Load<GameObject>(
                Path.Combine($@"{pathToPrefabs}/Warrior/Dwarf MasterM"));
            EnemyWarriorPrefab =  Resources.Load<GameObject>(
                Path.Combine($@"{pathToPrefabs}/Warrior/Dwarf BerserkerM"));
        }
    }
}
