using System;
using System.Collections.Generic;
using Leopotam.Ecs;

namespace Components
{
    public class PlayerComponent
    {
        public List<EcsEntity> SelectedUnits;
        public Guid Guid { get; }
        public long Score { get; set; } = 500;
        
        public PlayerComponent(Guid guid)
        {
            Guid = guid;
        }
    }
}