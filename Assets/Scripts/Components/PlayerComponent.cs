using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using System;

namespace Components
{
    public class PlayerComponent
    {
        public List<EcsEntity> SelectedUnits = new List<EcsEntity>();
        public Guid Guid { get; }
        public long Score { get; set; } = 500;
        
        public PlayerComponent(Guid guid)
        {
            Guid = guid;
        }
    }
}