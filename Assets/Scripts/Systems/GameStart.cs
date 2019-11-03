using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

public class GameStart : MonoBehaviour
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

    // Update is called once per frame
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
