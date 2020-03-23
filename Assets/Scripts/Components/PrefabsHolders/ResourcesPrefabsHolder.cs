using System.IO;
using UnityEngine;

namespace Components
{
    public class MoneyBagPrefabHolder
    {
        public static readonly GameObject MoneyBagPrefab;
        private const string pathToPrefab = @"Prefabs/MapResources";
        
        static MoneyBagPrefabHolder()
        {
            MoneyBagPrefab = Resources.Load<GameObject>(Path.Combine($@"{pathToPrefab}/MoneyBag"));
        }
    }
}