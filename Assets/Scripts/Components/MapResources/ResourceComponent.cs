using System;
using UnityEngine;

namespace Components
{
    public class ResourceComponent
    {
        public GameObject Object { get; set; }
        public Vector3 Position { get; set; }
        public ResourceTag Tag { get; set; }
        public int ResourceCount { get; set; }
        public Guid Guid { get; set; }
    }
}