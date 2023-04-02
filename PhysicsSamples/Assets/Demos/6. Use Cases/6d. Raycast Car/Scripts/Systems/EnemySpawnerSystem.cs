using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class EnemySpawnerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        EntityQuery enemyEntityQuery = EntityManager.CreateEntityQuery(typeof(EnemyTag));

        EnemySpawnerComponent enemySpawnerComponent = SystemAPI.GetSingleton<EnemySpawnerComponent>();
        RefRW<RandomComponent> randomComponent = SystemAPI.GetSingletonRW<RandomComponent>();

        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);

        int spawnAmount = 50;
        if (enemyEntityQuery.CalculateEntityCount() < spawnAmount)
        {
            Entity spawnedEntity = entityCommandBuffer.Instantiate(enemySpawnerComponent.enemyPrefab);
            entityCommandBuffer.SetComponent(spawnedEntity, new CharectorData
            {
                walkSpeed = randomComponent.ValueRW.random.NextFloat(1f, 5f)
            });
        }
    }
}
