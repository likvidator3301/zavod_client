using System.Collections.Generic;
using Entities;
using UnityEditor;

namespace Components
{
    public class PlayerComponent
    {
        public List<IUnitEntity> Units = new List<IUnitEntity>();

        public PlayerComponent(GUID guid)
        {
            Guid = guid;
        }

        public GUID Guid { get; }
        public long Score { get; set; } = 500;
    }
}