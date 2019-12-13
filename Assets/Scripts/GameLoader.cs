using UnityEngine;
using Leopotam.Ecs;
using Systems;
using System;
using Components;

public class GameLoader : MonoBehaviour
{
    public GameDefinitions gameDefinitions;

    private EcsWorld world;
    private EcsSystems systems;


    public void Start()
    {
        world = new EcsWorld();
        var playerComponent = new PlayerComponent(Guid.NewGuid());

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
#endif  

        var controlsSystems = new EcsSystems(world)
            .Add(new BuildCreateSystem())
            .Add(new InputSystem())
            .Add(new CameraSystem())
            .Add(new UnitActionHandler())
            .Add(new SelectionHandler())
            .Add(new CheckClickOnBuildsSystem());
        var levelSystems = new EcsSystems(world)
            .Add(new StartupTestLevelSystem())
            .Add(new LoadSystem());
        var unitSystems = new EcsSystems(world)
            .Add(new UnitStateChangeSystem())
            .Add(new UnitActionSystem())
            .Add(new UnitCreateSystem())
            .Add(new UnitAnimationSystem());

        systems = new EcsSystems(world)
            .Add(new GuiSystem())
            .Add(new ResoursesDisplaySystem())
            .Add(controlsSystems)
            .Add(unitSystems)
            .Add(levelSystems)
            .Inject(playerComponent)
            .Inject(gameDefinitions)
            .ProcessInjects();

        systems.Init();

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(systems);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(controlsSystems);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(levelSystems);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(unitSystems);
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
