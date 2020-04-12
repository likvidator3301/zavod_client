using System;
using UnityEngine;
using Leopotam.Ecs;
using Systems;
using Systems.Base;
using Systems.Controls.UnitControlSystem;
using Systems.Zavod;
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
            .Add(new UnitStartMoveSystem())
            .Add(new UnitMoveSystem())
            .Add(new UnitStartFollowSystem())
            .Add(new UnitFollowSystem())
            .Add(new UnitFindAvailableFightsSystem())
            .Add(new UnitStartAttackSystem())
            .Add(new UnitAttackSystem())
            .Add(new UnitChangeHealthSystem());

        var resourcesSystems = new EcsSystems(world)
            .Add(new ResourceCreateSystem())
            .Add(new ResourceFindAvailableToTakeSystem())
            .Add(new ResourceTakeSystem());

        var serverIntegrationSystems = new EcsSystems(world)
            .Add(new ClientSystem());

        var uiSystems = new EcsSystems(world)
            .Add(new ResoursesDisplaySystem())
            .Add(new ResourcesCollectorSystem())
            .Add(new KioskInitSystem())
            .Add(new UnitLayoutUISystem())
            .Add(new ConsolePrintSystem())
            .Add(new MessagesReceiverSystem());
        
        var zavodSystems = new EcsSystems(world)
            .Add(new GenerateMoneySystem());
        
        var baseSystems = new EcsSystems(world)
            .Add(new FindNearDeliversToTakeResourcesSystem())
            .Add(new TakeResourcesFromDeliverSystem());
        
        var destroySystem = new EcsSystems(world)
            .Add(new DestroySystem());


        systems = new EcsSystems(world)
            .Add(controlsSystems)
            .Add(levelSystems)
            .Add(unitSystems)
            .Add(unitControlsSystems)
            .Add(zavodSystems)
            .Add(resourcesSystems)
            .Add(baseSystems)
            .Add(serverIntegrationSystems)
            .Add(uiSystems)
            .Add(destroySystem)
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
