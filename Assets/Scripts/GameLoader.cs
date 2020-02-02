using System;
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
        var serverIntegration = new ServerIntegration.ServerIntegration();

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
#endif  

        systems = new EcsSystems(world)
            .Add(new LoadSystem())
            .Add(new CameraSystem())
            .Add(new InputSystem())
            .Add(new CheckClickOnBuildsSystem())
            .Add(new BuildCreateSystem())
            .Add(new ButtonsClickSystem())
            .Add(new ResoursesDisplaySystem())
            .Add(new ResourcesCollectorSystem())
            .Add(new UnitStateChangeSystem())
            .Add(new UnitActionSystem())
            .Add(new UnitCreateSystem())
            .Add(new UnitAnimationSystem())
            .Add(new UnitActionHandler())
            .Add(new SelectionHandler())
            .Add(new UnitLayoutUISystem())
            .Add(new UnitHealthSystem())
            .Add(new ClientSystem())
            .Add(new GuiSystem())
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
