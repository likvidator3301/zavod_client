using System;
using UnityEngine;
using Leopotam.Ecs;
using Systems;
using Systems.Communication;
using Components;
using Models;

public class GameLoader : MonoBehaviour
{
    public GameDefinitions gameDefinitions;

    private EcsWorld world;
    private EcsSystems systems;


    public void Start()
    {
        world = new EcsWorld();
        var playerComponent = new PlayerComponent(Guid.NewGuid());
        playerComponent.SelectedUnits = new System.Collections.Generic.List<EcsEntity>();

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
            .Add(new UnitStateChangeSystem())
            .Add(new UnitActionSystem())
            .Add(new UnitCreateSystem())
            .Add(new UnitAnimationSystem())
            .Add(new UnitHealthSystem());

        var uiSystems = new EcsSystems(world)
            .Add(new ResoursesDisplaySystem())
            .Add(new ResourcesCollectorSystem())
            .Add(new KioskInitSystem())
            .Add(new UnitLayoutUISystem())
            .Add(new ConsolePrintSystem())
            .Add(new MessagesReceiverSystem())
            .Add(new OpenPauseMenuSystem());

        var serverSystem = new EcsSystems(world)
            .Add(new CommunicationInitSystem());


        systems = new EcsSystems(world)
            .Add(controlsSystems)
            .Add(levelSystems)
            .Add(unitSystems)
            .Add(uiSystems)
            //.Add(serverSystem)
            .Inject(playerComponent)
            .Inject(gameDefinitions)
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
