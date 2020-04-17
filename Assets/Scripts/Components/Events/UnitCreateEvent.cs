using UnityEngine;
using System;

namespace Components
{
    internal class UnitCreateEvent
    {
        public UnitTag UnitTag;
        public Vector3 Position;
        public Guid Id;
        public Guid PlayerId;
        public int Health;
    }
}
