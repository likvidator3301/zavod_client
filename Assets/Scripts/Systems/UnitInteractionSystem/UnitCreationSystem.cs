using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreationSystem : IEcsRunSystem
{
    EcsWorld world = null;

    public void Run()
    {
        if (Input.GetKeyDown("q"))
        {
            world.NewEntityWith<Unit>(out var unit);
            unit.Id = Random.Range(1f, 10f); // setting random id for unit
            world.NewEntityWith<UnitCreationEvent>(out var createdUnit);
            createdUnit.Unit = unit;
        }
    }
}
