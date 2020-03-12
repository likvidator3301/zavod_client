using System;
using UnityEngine;
using Leopotam.Ecs;
using Systems;
using Systems.Controls.UnitControlSystem;
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
            .Add(new BuildingPlaceSelectionSystem())
            .Add(new BuildModeVisualisationSystem())
            .Add(new BuildingInstallValidatorSystem())
            .Add(new InstallBuildingSystem())
            .Add(new BuildingRotationSystem())
            .Add(new BuildExitSystem())
            .Add(new InputSystem())
            .Add(new CameraSystem())
            .Add(new UnitActionHandler())
            .Add(new SelectionHandler())
            .Add(new CheckClickOnBuildsSystem())
            .Add(new ButtonsClickSystem());

        var levelSystems = new EcsSystems(world)
            .Add(new StartupTestLevelSystem())
            .Add(new LoadSystem());

        var unitSystems = new EcsSystems(world)
            .Add(new UnitCreateSystem());
        
        var unitControlsSystems = new EcsSystems(world)
            .Add(new UnitMoveSystem())
            .Add(new UnitFollowSystem())
            .Add(new UnitAttackSystem())
            .Add(new HealthSystem());

        var serverIntegrationSystems = new EcsSystems(world)
            .Add(new ClientSystem());

        var uiSystems = new EcsSystems(world)
            .Add(new ResoursesDisplaySystem())
            .Add(new ResourcesCollectorSystem())
            .Add(new KioskInitSystem())
            .Add(new UnitLayoutUISystem())
            .Add(new ConsolePrintSystem())
            .Add(new MessagesReceiverSystem());


        systems = new EcsSystems(world)
            .Add(controlsSystems)
            .Add(levelSystems)
            .Add(unitSystems)
            .Add(unitControlsSystems)
            .Add(serverIntegrationSystems)
            .Add(uiSystems)
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
