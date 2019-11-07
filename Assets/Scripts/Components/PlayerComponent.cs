using System.Collections.Generic;
using UnityEditor;

namespace Components
{
    public class PlayerComponent
    {
        public List<IUnitEntity> SelectedUnits = new List<IUnitEntity>();

        public PlayerComponent(GUID guid)
        {
            Guid = guid;
        }

        public GUID Guid { get; }
        public long Score { get; set; } = 500;
    }
}