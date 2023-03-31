using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject enemyPrefab;
}

public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
{
    public override void Bake(EnemySpawnerAuthoring authoring)
    {
        AddComponent(new EnemySpawnerComponent
        {
            enemyPrefab = GetEntity(authoring.enemyPrefab)
        });
    }
}
