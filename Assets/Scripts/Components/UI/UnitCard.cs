using Leopotam.Ecs;
using UnityEngine;

public class UnitCard
{
    [EcsIgnoreNullCheck]
    public GameObject Prefab = Resources.Load<GameObject>("Prefabs/UnitIcon");

    [EcsIgnoreNullCheck]
    public Unit Unit;
}
