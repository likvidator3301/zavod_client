using Leopotam.Ecs;
using UnityEngine;

namespace Components.Attack
{
    public class StartAttackEvent
    {
        public EcsEntity TargetEntity { get; set; }
    }
}