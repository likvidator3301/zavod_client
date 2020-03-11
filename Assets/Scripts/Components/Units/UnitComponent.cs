using System;
using UnityEngine;
using UnityEngine.AI;

namespace Components
{
    public class UnitComponent
    {
        public GameObject Object { get; set; }
        public UnitTag Tag { get; set; }
        public Guid Guid { get; set; }
    }
}