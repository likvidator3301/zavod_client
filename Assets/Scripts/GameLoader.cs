using System;
using UnityEngine;
using Leopotam.Ecs;
using Systems;
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
        var serverIntegration = new ServerIntegration.ServerIntegration();

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
            .Add(new UnitAnimationSystem())
            .Add(new UnitHealthSystem());
        var serverIntegrationSystems = new EcsSystems(world)
            .Add(new ClientSystem());

        systems = new EcsSystems(world)
            .Add(new GuiSystem())
            .Add(controlsSystems)
            .Add(unitSystems)
            .Add(levelSystems)
            .Add(serverIntegrationSystems)
            .Inject(playerComponent)
            .Inject(gameDefinitions)
            .Inject(serverIntegration)
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
