using System.IO;
using UnityEngine;

namespace Components
{
    public class ZavodPrefabHolder
    {
        public static readonly GameObject ZavodPrefab;
        private const string pathToPrefab = @"Prefabs/Buildings/Zavod";
        
        static ZavodPrefabHolder()
        {
            ZavodPrefab = Resources.Load<GameObject>(Path.Combine($@"{pathToPrefab}"));
        }
    }
}