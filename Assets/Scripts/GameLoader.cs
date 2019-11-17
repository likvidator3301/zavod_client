﻿using UnityEngine;
using Leopotam.Ecs;
using Systems;
using Components;
using UnityEditor;

public class GameLoader : MonoBehaviour
{
    public GameObject[] builds;
    public GameDefinitions gameDefs;
    public PrefabsHolderComponent PrefabsHolder;

    private EcsWorld world;
    private EcsSystems systems;


    public void Start()
    {
        world = new EcsWorld();
        var playerComponent = new PlayerComponent(GUID.Generate());

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
#endif  

        var controlsSystems = new EcsSystems(world)
            .Add(new BuildCreateSystem())
            .Add(new InputSystem())
            .Add(new CameraSystem())
            .Add(new UnitActionHandler())
            .Add(new SelectionHandler());
        var levelSystems = new EcsSystems(world)
            .Add(new StartupTestLevelSystem());
        var unitSystems = new EcsSystems(world)
            .Add(new UnitStateChangeSystem())
            .Add(new UnitActionSystem());
        systems = new EcsSystems(world)
            .Add(controlsSystems)
            .Add(unitSystems)
            .Add(levelSystems)
            .Inject(playerComponent)
            .Inject(PrefabsHolder)
            .Inject(builds)
            .Inject(gameDefs)
            .ProcessInjects();

        systems.Init();

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
