using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Components.Zavod
{
    public class ZavodComponent
    {
        public GameObject Object { get; set; }
        public MapBuildingTag Tag { get; set; }
        public Vector3 Position { get; set; }
        public Guid Guid { get; set; }
    }
}