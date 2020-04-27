using System;
using UnityEngine;
using Leopotam.Ecs;
using Systems;
using Systems.Base;
using Systems.Communication;
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
            .Add(new ButtonsClickSystem())
            .Add(new UnitFindAvailableBuildingSystem())
            .Add(new BuildHpUpdateSystem())
            .Add(new FindDeathBuildingSystem());

        var levelSystems = new EcsSystems(world)
            .Add(new StartupTestLevelSystem())
            .Add(new LoadSystem())
            .Add(new ExitGameSystem())
            .Add(new ExitToMainMenuSystem())
            .Add(new CheckSessionExistSystem());

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
            .Add(new FindDeadUnitsSystem());

        var resourcesSystems = new EcsSystems(world)
            .Add(new ResourceCreateSystem())
            .Add(new ResourceFindAvailableToTakeSystem())
            .Add(new ResourceTakeSystem())
            .Add(new DropResourceAfterDeathSystem());

        var uiSystems = new EcsSystems(world)
            .Add(new ResoursesDisplaySystem())
            .Add(new ResourcesCollectorSystem())
            .Add(new KioskInitSystem())
            //.Add(new UnitLayoutUISystem())
            .Add(new ConsolePrintSystem())
            .Add(new MessagesReceiverSystem())
            .Add(new OpenPauseMenuSystem());

        var serverSystem = new EcsSystems(world)
            .Add(new CommunicationInitSystem())
            .Add(new UpdateEnemyUnitsSystem())
            .Add(new UnitPositionsSendSystem())
            .Add(new BuildingsPositionSendSystem())
            .Add(new UpdateEnemyBuildingsSystem())
            .Add(new UnitsHpUpdate())
            .Add(new BagsCreateSystem())
            .Add(new BagsRemoveSystem());
        
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
            .Add(uiSystems)
            .Add(baseSystems)
            .Add(destroySystem)
            .Add(serverSystem)
            .Inject(gameDefinitions)
            .Inject(playerComponent)
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
