using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreationSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    EcsWorld world = null;

    void IEcsInitSystem.Init()
    {

    }

    void IEcsRunSystem.Run()
    {
        if (Input.GetKeyDown("q"))
        {
            world.NewEntityWith<Unit>(out var unit);
            unit.id = Random.Range(1f, 10f);
            world.NewEntityWith<UnitCreationEvent>(out var createdUnit);
            createdUnit.unit = unit;
        }
    }
    
    void IEcsDestroySystem.Destroy()
    {

    }
}
