using Leopotam.Ecs;
using UnityEngine;

namespace Components.Attack
{
    public class StartAttackingEvent
    {
        public EcsEntity TargetEntity { get; set; }
    }
}