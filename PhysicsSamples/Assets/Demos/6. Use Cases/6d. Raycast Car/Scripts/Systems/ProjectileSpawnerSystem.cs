using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public partial class ProjectileSpawnerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        EntityQuery enemyEntityQuery = EntityManager.CreateEntityQuery(typeof(ProjectileTag));

        ProjectileSpawnerComponent projectileSpawnerComponent = SystemAPI.GetSingleton<ProjectileSpawnerComponent>();

        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);

        int spawnAmount = 1;
        if (enemyEntityQuery.CalculateEntityCount() < spawnAmount)
        {
            //Entity spawnedEntity = entityCommandBuffer.Instantiate(projectileSpawnerComponent.projectilePrefab);
        }
    }
}
