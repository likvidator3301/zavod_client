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
        public List<IUnitEntity> Units = new List<IUnitEntity>();

        public PlayerComponent(GUID guid)
        {
            this.Guid = guid;
        }
    }
}
