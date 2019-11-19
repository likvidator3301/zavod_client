using System.IO;
using UnityEngine;

namespace Components
{
    public static class UnitsInterfacesHolder
    {
        public static readonly GameObject SelectionFrame;
        private const string pathToPrefabs = @"Prefabs/UnitsInterfaces";

        static  UnitsInterfacesHolder()
        {
            SelectionFrame = Resources.Load<GameObject>(
                Path.Combine($@"{pathToPrefabs}/SelectionFrame"));
        }
    }
}