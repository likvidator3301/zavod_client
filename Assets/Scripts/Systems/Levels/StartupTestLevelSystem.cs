namespace Systems
{
    using System.Threading.Tasks;

    using Components;

    using Leopotam.Ecs;

    using Models;

    using Vector3 = UnityEngine.Vector3;

    public class StartupTestLevelSystem : IEcsInitSystem
    {
        private ServerIntegration.ServerIntegration serverIntegration;
        private const float minHeight = 2.6f;
        private EcsWorld ecsWorld;
        private EcsGrowList<UnitComponent> units;
        private readonly Vector3 allyUnitPosition = new Vector3(44, minHeight, 40);
        private readonly Vector3 enemyUnitPosition = new Vector3(44, minHeight, 45);

        public void Init()
        {
            InitializeLevel();
        }

        private async Task InitializeLevel()
        {
            var allyUnitDto = new CreateUnitDto();
            var enemyUnitDto = new CreateUnitDto();
            allyUnitDto.Position = new Models.Vector3(){X=allyUnitPosition.x, Y=allyUnitPosition.y, Z=allyUnitPosition.z};
            enemyUnitDto.Position = new Models.Vector3(){X=enemyUnitPosition.x, Y=enemyUnitPosition.y, Z=enemyUnitPosition.z};
            
            int timeout = 1000;
            var task = serverIntegration.client.Unit.CreateUnit(allyUnitDto);

            ServerUnitDto allyUnit;
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                allyUnit = task.Result;
            }

            var enemyTask = serverIntegration.client.Unit.CreateUnit(enemyUnitDto);
            ServerUnitDto enemyUnit;
            if (await Task.WhenAny(enemyTask, Task.Delay(timeout)) == enemyTask)
            {
                enemyUnit = enemyTask.Result;
            }

            UnitsPrefabsHolder.WarriorPrefab.AddNewUnitEntityOnPositionFromUnitDbo(
                ecsWorld, allyUnitPosition, allyUnitDto);
            UnitsPrefabsHolder.WarriorPrefab.AddNewUnitEntityOnPositionFromUnitDbo(
                ecsWorld, enemyUnitPosition, enemyUnitDto);
        }
    }
}