using System.Collections.Generic;
using UnityEditor;

namespace Components
{
    public class PlayerComponent
    {
        public List<UnitComponent> SelectedUnits = new List<UnitComponent>();
        public GUID Guid { get; }
        public long Score { get; set; } = 500;
        
        public PlayerComponent(GUID guid)
        {
            Guid = guid;
        }
    }
}