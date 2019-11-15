using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using Systems;
using Components;

public class GameLoader : MonoBehaviour
{
    public GameObject[] builds;
    public GameDefinitions gameDefs;

    EcsWorld world;
    EcsSystems systems;

    void Start()
    {
        world = new EcsWorld();
        systems = new EcsSystems(world);

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
#endif  
        systems
            .Add(new BuildCreateSystem())
            .Add(new InputSystem())
            .Add(new CameraSystem())
            .Inject(builds)
            .Inject(gameDefs)
            .ProcessInjects()
            .Init();

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(systems);
#endif
    }

    void Update()
    {
        systems.Run();
        world.EndFrame();
    }

    void OnDestroy()
    {
        systems.Destroy();
        world.Destroy();
    }
}
