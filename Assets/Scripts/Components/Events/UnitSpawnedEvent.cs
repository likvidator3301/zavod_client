using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using Components;

public class UnitSpawnedEvent
{
    [EcsIgnoreNullCheck]
    public EcsEntity Unit;
}
