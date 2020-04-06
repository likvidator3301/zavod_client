using System;
using UnityEngine;

namespace Components.Base
{
    public class BaseComponent
    {
        public GameObject Object { get; set; }
        public MapBuildingTag Tag { get; set; }
        public Vector3 Position { get; set; }
        public Guid Guid { get; set; }
    }
}