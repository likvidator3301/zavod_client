using System.IO;
using UnityEngine;

namespace Components
{
    public class MapBuildingsPrefabsHolder
    {
        public static readonly GameObject ZavodPrefab;
        public static readonly GameObject BasePrefab;
        private const string pathToZavodPrefab = @"Prefabs/Buildings/Zavod";
        private const string pathToBasePrefab = @"Prefabs/Buildings/Base";
        
        static MapBuildingsPrefabsHolder()
        {
            ZavodPrefab = Resources.Load<GameObject>(Path.Combine($@"{pathToZavodPrefab}"));
            BasePrefab = Resources.Load<GameObject>(Path.Combine($@"{pathToBasePrefab}"));
        }
    }
}