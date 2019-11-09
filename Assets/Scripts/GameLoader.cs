using UnityEngine;
using Leopotam.Ecs;
using Systems;
using Components;

public class GameLoader : MonoBehaviour
{
    public GameObject[] builds;
    public GameDefinitions gameDefs;
    public PrefabsHolderComponent PrefabsHolder;

    private EcsWorld world;
    private EcsSystems systems;
    private PressedKeysBuffer pressedKeys;


    public void Start()
    {
        world = new EcsWorld();
        systems = new EcsSystems(world);
        pressedKeys = new PressedKeysBuffer();

        var controlsSystems = new EcsSystems(world)
            .Add(new BuildCreateSystem())
            .Add(new InputSystem())
            .Add(new CameraSystem())
            .Add(new UnitActionHandler())
            .Add(new SelectionHandler());
        var levelSystems = new EcsSystems(world)
            .Add(new StartupTestLevelSystem());
        var unitSystems = new EcsSystems(world)
            .Add(new UnitConditionChangeSystem())
            .Add(new UnitActionSystem());

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
#endif  
        systems
            .Add(controlsSystems)
            .Add(unitSystems)
            .Add(levelSystems)
            .Inject(new PlayerComponent())
            .Inject(new WorldComponent())
            .Inject(PrefabsHolder)
            .Inject(builds)
            .Inject(gameDefs)
            .Inject(pressedKeys)
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
