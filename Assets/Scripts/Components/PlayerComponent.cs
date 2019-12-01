using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEditor;

namespace Components
{
    public class PlayerComponent
    {
        public List<EcsEntity> SelectedUnits = new List<EcsEntity>();
        public GUID Guid { get; }
        public long Score { get; set; } = 500;
        
        public PlayerComponent(GUID guid)
        {
            Guid = guid;
        }
    }
}