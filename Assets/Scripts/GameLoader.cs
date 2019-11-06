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
    private PressedKeysBuffer pressedKeys;

    void Start()
    {
        world = new EcsWorld();
        systems = new EcsSystems(world);
        pressedKeys = new PressedKeysBuffer();

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
#endif  
        systems
            .Add(new BuildCreateSystem())
            .Add(new InputSystem())
            .Add(new CameraSystem())
            .Add(new UnitActionSystem())
            .Add(new UnitConditionChangeSystem())
            .Add(new StartupTestLevelSystem())
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
