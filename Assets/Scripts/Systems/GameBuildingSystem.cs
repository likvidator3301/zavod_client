using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

public class GameBuildingSystem : MonoBehaviour
{
    EcsWorld world;

    EcsSystems systems;
        void Start()
    {
        world = new EcsWorld();
        systems = new EcsSystems(world)
            .Add(new UnitCreationSystem())
            .Add(new UnitListDisplaySystem())
            .Add(new UnitSelectionSystem());
        systems.Init();
    }

    void Update()
    {
        systems.Run();
        world.EndFrame();
    }

    void OnDestroy()
    {
        // destroy systems logical group.
        systems.Destroy();
        // destroy world.
        world.Destroy();
    }
}
