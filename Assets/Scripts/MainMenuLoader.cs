using Leopotam.Ecs;
using UnityEngine;
using Systems;

public class MainMenuLoader : MonoBehaviour
{
    private EcsWorld world;
    private EcsSystems systems;

    void Start()
    {
        world = new EcsWorld();

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(world);
#endif  
        systems = new EcsSystems(world)
            .Add(new LoadMainMenuSystem())
            .Add(new MainMenuButttonsSystem());

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
