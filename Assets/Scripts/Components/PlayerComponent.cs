using System.Collections.Generic;
using UnityEditor;

namespace Components
{
    public class PlayerComponent
    {
        public List<IUnitEntity> HighlightedUnits = new List<IUnitEntity>();

        public PlayerComponent(GUID guid)
        {
            Guid = guid;
        }

        public GUID Guid { get; }
        public long Score { get; set; } = 500;
    }
}