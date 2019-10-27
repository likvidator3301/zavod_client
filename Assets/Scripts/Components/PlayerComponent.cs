using System.Collections.Generic;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Components
{
    public class PlayerComponent
    {
        public GUID Guid { get; }
        public long Score { get; set; } = 500;
        public List<GameObject> PlayerUnits = new List<GameObject>();

        public PlayerComponent(GUID guid)
        {
            this.Guid = guid;
        }
    }
}
