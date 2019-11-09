using System.Collections.Generic;
using UnityEditor;

namespace Components
{
    public class PlayerComponent
    {
        public List<IUnitEntity> SelectedUnits;

        public PlayerComponent(GUID guid)
        {
            Guid = guid;
        }

        public PlayerComponent()
        {
            SelectedUnits = new List<IUnitEntity>();
        }

        public GUID Guid { get; }
        public long Score { get; set; } = 500;
    }
}